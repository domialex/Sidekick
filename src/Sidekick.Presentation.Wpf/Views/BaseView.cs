using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AdonisUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Wpf.Views
{
    public abstract class BaseView : AdonisWindow, ISidekickView
    {
        protected readonly ISidekickSettings settings;
        protected readonly IViewPreferenceRepository viewPreferenceRepository;
        protected readonly ILogger logger;
        protected readonly IMouseProvider mouseProvider;
        protected readonly IScreenProvider screenProvider;

        protected BaseView()
        {
            // An empty constructor is necessary for the designer to show a preview
        }

        protected BaseView(View view, IServiceProvider serviceProvider)
        {
            settings = serviceProvider.GetService<ISidekickSettings>();
            mouseProvider = serviceProvider.GetService<IMouseProvider>();
            screenProvider = serviceProvider.GetService<IScreenProvider>();
            viewPreferenceRepository = serviceProvider.GetService<IViewPreferenceRepository>();
            logger = serviceProvider.GetService<ILogger<BaseView>>();

            IsVisibleChanged += EnsureBounds;
            Loaded += EnsureBounds;
            Loaded += BaseWindow_Loaded;
            SizeChanged += EnsureBounds;

            View = view;
        }

        public Domain.Views.View View { get; }

        public virtual Task Open(params object[] args)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                {
                    Show();
                    Activate();
                });
                return Task.CompletedTask;
            }

            Show();
            Activate();
            return Task.CompletedTask;
        }

        public new void Close()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => base.Close());
                return;
            }

            base.Close();
        }

        public new void Hide()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => base.Hide());
                return;
            }

            base.Hide();
        }

        protected bool IsClosing = false;
        protected override async void OnClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
            {
                try
                {
                    await viewPreferenceRepository.SaveSize(View, GetWidth(), GetHeight());
                }
                catch (ObjectDisposedException)
                {
                    // Catches, if the service provider is being disposed.
                    // We keep going
                }
            }

            IsClosing = true;
            IsVisibleChanged -= EnsureBounds;
            Loaded -= EnsureBounds;
            Loaded -= BaseWindow_Loaded;
            SizeChanged -= EnsureBounds;

            base.OnClosing(e);
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
            {
                Task.Run(async () =>
                {
                    var preferences = await viewPreferenceRepository.Get(View);
                    if (preferences != null)
                    {
                        var previousWidth = GetWidth();
                        var previousHeight = GetHeight();
                        SetWidth(preferences.Width);
                        SetHeight(preferences.Height);

                        if (LeftLocationSource == LocationSource.Center)
                        {
                            MoveX((previousWidth - preferences.Width) / 2);
                        }
                        else if (LeftLocationSource == LocationSource.End)
                        {
                            MoveX(previousWidth - preferences.Width);
                        }

                        if (TopLocationSource == LocationSource.Center)
                        {
                            MoveY((previousHeight - preferences.Height) / 2);
                        }
                        else if (TopLocationSource == LocationSource.End)
                        {
                            MoveY(previousHeight - preferences.Height);
                        }

                        EnsureBounds();
                    }
                });
            }
        }

        public new void Show()
        {
            base.Show();
            EnsureBounds();
        }

        private LocationSource TopLocationSource = LocationSource.Begin;
        protected void SetTopPercent(double y, LocationSource source = LocationSource.Begin)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetTopPercent(y, source));
                return;
            }

            if (y > 1) { y /= 100; }

            if (source == LocationSource.Center)
            {
                y -= GetHeightPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                y -= GetHeightPercent();
            }

            var screenRect = screenProvider.GetBounds();
            var desiredY = screenRect.Y + (screenRect.Height * y);

            TopLocationSource = source;
            Top = (int)desiredY;
            EnsureBounds();
        }

        private LocationSource LeftLocationSource = LocationSource.Begin;
        protected void SetLeftPercent(double x, LocationSource source = LocationSource.Begin)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetLeftPercent(x, source));
                return;
            }

            if (x > 1) { x /= 100; }

            if (source == LocationSource.Center)
            {
                x -= GetWidthPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                x -= GetWidthPercent();
            }

            var screenRect = screenProvider.GetBounds();
            var desiredX = screenRect.X + (screenRect.Width * x);

            LeftLocationSource = source;
            Left = (int)desiredX;
            EnsureBounds();
        }

        protected void EnsureBounds()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => EnsureBounds());
                return;
            }

            if (IsVisible)
            {
                var screenRect = screenProvider.GetBounds();

                // Is off to the right
                if (Left + GetWidth() > screenRect.X + screenRect.Width)
                {
                    Left = screenRect.X + screenRect.Width - GetWidth();
                }

                // Is off to the left
                if (Left < screenRect.X)
                {
                    Left = screenRect.X;
                }

                // Is off to the top
                if (Top < screenRect.Y)
                {
                    Top = screenRect.Y;
                }

                // Is off to the bottom
                if (Top + GetHeight() > screenRect.Y + screenRect.Height)
                {
                    Top = screenRect.Y + screenRect.Height - GetHeight();
                }
            }
        }
        private void EnsureBounds(object sender, DependencyPropertyChangedEventArgs e) => EnsureBounds();
        private void EnsureBounds(object sender, EventArgs e) => EnsureBounds();

        private double GetWidth()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetWidth());
            }

            return ActualWidth;
        }

        private double GetWidthPercent()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetWidthPercent());
            }

            var screenRect = screenProvider.GetBounds();
            return ActualWidth / screenRect.Width;
        }

        private double GetHeight()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetHeight());
            }

            return ActualHeight;
        }

        private double GetHeightPercent()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetHeightPercent());
            }

            var screenRect = screenProvider.GetBounds();
            return ActualHeight / screenRect.Height;
        }

        private void SetWidth(double width)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetWidth(width));
                return;
            }

            Width = width;
        }

        private void SetHeight(double height)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetHeight(height));
                return;
            }

            Height = height;
        }

        protected double GetMouseXPercent()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetMouseXPercent());
            }

            var mousePosition = mouseProvider.GetPosition();
            var screenRect = screenProvider.GetBounds();

            return (double)(mousePosition.X - screenRect.X) / screenRect.Width;
        }

        protected void MoveX(double x)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => MoveX(x));
                return;
            }

            Left += x;
        }

        protected void MoveY(double y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => MoveY(y));
                return;
            }

            Top += y;
        }
    }
}
