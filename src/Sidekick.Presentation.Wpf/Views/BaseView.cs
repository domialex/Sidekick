using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using AdonisUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using MyCursor = System.Windows.Forms.Cursor;

namespace Sidekick.Presentation.Wpf.Views
{
    public abstract class BaseView : AdonisWindow, ISidekickView
    {
        protected readonly ISidekickSettings settings;
        protected readonly IViewPreferenceRepository viewPreferenceRepository;
        protected readonly ILogger logger;

        protected BaseView()
        {
            // An empty constructor is necessary for the designer to show a preview
        }

        protected BaseView(Domain.Views.View view, IServiceProvider serviceProvider)
        {
            settings = serviceProvider.GetService<ISidekickSettings>();
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

            logger.LogInformation($"Positioning Info: SetTopPercent({y}, {source})");

            if (source == LocationSource.Center)
            {
                y -= GetHeightPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                y -= GetHeightPercent();
            }

            logger.LogInformation($"Positioning Info: SetTopPercent: y = {y}");
            logger.LogInformation($"Positioning Info: SetTopPercent: ActualHeight = {ActualHeight}");
            logger.LogInformation($"Positioning Info: SetTopPercent: GetHeightPercent() = {GetHeightPercent()}");

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

            var desiredY = screenRect.Y + (screenRect.Height * y);

            logger.LogInformation($"Positioning Info: SetTopPercent: Screen.Bounds.Height = {screenRect.Height}");
            logger.LogInformation($"Positioning Info: SetTopPercent: Screen.Bounds.Y = {screenRect.Y}");
            logger.LogInformation($"Positioning Info: SetTopPercent: Top = {desiredY}");

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

            logger.LogInformation($"Positioning Info: SetLeftPercent({x}, {source})");

            if (source == LocationSource.Center)
            {
                x -= GetWidthPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                x -= GetWidthPercent();
            }

            logger.LogInformation($"Positioning Info: SetLeftPercent: x = {x}");
            logger.LogInformation($"Positioning Info: SetLeftPercent: ActualWidth = {ActualWidth}");
            logger.LogInformation($"Positioning Info: SetLeftPercent: GetWidthPercent() = {GetWidthPercent()}");

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

            var desiredX = screenRect.X + (screenRect.Width * x);

            logger.LogInformation($"Positioning Info: SetLeftPercent: Screen.Bounds.Width = {screenRect.Width}");
            logger.LogInformation($"Positioning Info: SetLeftPercent: Screen.Bounds.X = {screenRect.X}");
            logger.LogInformation($"Positioning Info: SetLeftPercent: Left = {desiredX}");

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
                var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

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

        protected double GetWidth()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetWidth());
            }

            return ActualWidth;
        }

        protected double GetWidthPercent()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetWidthPercent());
            }

            var screen = Screen.FromPoint(MyCursor.Position).Bounds;
            return ActualWidth / screen.Width;
        }

        protected double GetHeight()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetHeight());
            }

            return ActualHeight;
        }

        protected double GetHeightPercent()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetHeightPercent());
            }

            var screen = Screen.FromPoint(MyCursor.Position).Bounds;
            return ActualHeight / screen.Height;
        }

        protected void SetWidth(double width)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetWidth(width));
                return;
            }

            Width = width;
        }

        protected void SetHeight(double height)
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

            var screen = Screen.FromPoint(MyCursor.Position).Bounds;

            return (double)(MyCursor.Position.X - screen.X) / screen.Width;
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
