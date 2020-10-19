using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using AdonisUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sidekick.Business.Windows;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using MyCursor = System.Windows.Forms.Cursor;

namespace Sidekick.Views
{
    public abstract class BaseView : AdonisWindow, ISidekickView
    {
        private readonly IKeybindEvents keybindEvents;
        private readonly SidekickSettings settings;
        private readonly IWindowService windowService;
        private readonly ILogger logger;
        private readonly bool closeOnBlur;
        private readonly bool closeOnKey;
        private readonly string id;

        protected BaseView()
        {
            // An empty constructor is necessary for the designer to show a preview
        }

        protected BaseView(string id, IServiceProvider serviceProvider, bool closeOnBlur = false, bool closeOnKey = false)
        {
            keybindEvents = serviceProvider.GetService<IKeybindEvents>();
            settings = serviceProvider.GetService<SidekickSettings>();
            windowService = serviceProvider.GetService<IWindowService>();
            logger = serviceProvider.GetService<ILogger>();

            IsVisibleChanged += EnsureBounds;
            Loaded += EnsureBounds;
            Loaded += BaseWindow_Loaded;
            SizeChanged += EnsureBounds;

            if (closeOnBlur && settings.CloseOverlayWithMouse)
            {
                Deactivated += BaseBorderlessWindow_Deactivated;
            }

            if (closeOnKey)
            {
                keybindEvents.OnCloseWindow += KeybindEvents_OnCloseWindow;
            }

            this.closeOnBlur = closeOnBlur;
            this.closeOnKey = closeOnKey;
            this.id = id;
        }

        protected bool IsClosing = false;
        protected override async void OnClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            await windowService.SaveSize(id, GetWidth(), GetHeight());

            IsClosing = true;
            IsVisibleChanged -= EnsureBounds;
            Loaded -= EnsureBounds;
            Loaded -= BaseWindow_Loaded;
            SizeChanged -= EnsureBounds;

            if (closeOnBlur && settings.CloseOverlayWithMouse)
            {
                Deactivated -= BaseBorderlessWindow_Deactivated;
            }

            if (closeOnKey)
            {
                keybindEvents.OnCloseWindow -= KeybindEvents_OnCloseWindow;
            }

            base.OnClosing(e);
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var window = await windowService.Get(id);
                if (window != null)
                {
                    var previousWidth = GetWidth();
                    var previousHeight = GetHeight();
                    SetWidth(window.Width);
                    SetHeight(window.Height);

                    if (LeftLocationSource == LocationSource.Center)
                    {
                        MoveX((previousWidth - window.Width) / 2);
                    }
                    else if (LeftLocationSource == LocationSource.End)
                    {
                        MoveX(previousWidth - window.Width);
                    }

                    if (TopLocationSource == LocationSource.Center)
                    {
                        MoveY((previousHeight - window.Height) / 2);
                    }
                    else if (TopLocationSource == LocationSource.End)
                    {
                        MoveY(previousHeight - window.Height);
                    }

                    EnsureBounds();
                }
            });
        }

        private Task<bool> KeybindEvents_OnCloseWindow()
        {
            Close();
            return Task.FromResult(true);
        }

        private void BaseBorderlessWindow_Deactivated(object sender, EventArgs e)
        {
            Close();
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
                Dispatcher.Invoke(() => SetTopPercent(y));
                return;
            }

            if (y > 1) { y /= 100; }

            logger.Information($"Positioning Info: SetTopPercent({y}, {source})");

            if (source == LocationSource.Center)
            {
                y -= GetHeightPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                y -= GetHeightPercent();
            }

            logger.Information($"Positioning Info: SetTopPercent: y = {y}");
            logger.Information($"Positioning Info: SetTopPercent: ActualHeight = {ActualHeight}");
            logger.Information($"Positioning Info: SetTopPercent: GetHeightPercent() = {GetHeightPercent()}");

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

            var desiredY = screenRect.Y + (screenRect.Height * y);

            logger.Information($"Positioning Info: SetTopPercent: Screen.Bounds.Height = {screenRect.Height}");
            logger.Information($"Positioning Info: SetTopPercent: Screen.Bounds.Y = {screenRect.Y}");
            logger.Information($"Positioning Info: SetTopPercent: Top = {desiredY}");

            TopLocationSource = source;
            Top = (int)desiredY;
            EnsureBounds();
        }

        private LocationSource LeftLocationSource = LocationSource.Begin;
        protected void SetLeftPercent(double x, LocationSource source = LocationSource.Begin)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetLeftPercent(x));
                return;
            }

            if (x > 1) { x /= 100; }

            logger.Information($"Positioning Info: SetLeftPercent({x}, {source})");

            if (source == LocationSource.Center)
            {
                x -= GetWidthPercent() / 2;
            }
            else if (source == LocationSource.End)
            {
                x -= GetWidthPercent();
            }

            logger.Information($"Positioning Info: SetLeftPercent: x = {x}");
            logger.Information($"Positioning Info: SetLeftPercent: ActualWidth = {ActualWidth}");
            logger.Information($"Positioning Info: SetLeftPercent: GetWidthPercent() = {GetWidthPercent()}");

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

            var desiredX = screenRect.X + (screenRect.Width * x);

            logger.Information($"Positioning Info: SetLeftPercent: Screen.Bounds.Width = {screenRect.Width}");
            logger.Information($"Positioning Info: SetLeftPercent: Screen.Bounds.X = {screenRect.X}");
            logger.Information($"Positioning Info: SetLeftPercent: Left = {desiredX}");

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
