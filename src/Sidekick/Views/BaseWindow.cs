using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using AdonisUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using MyCursor = System.Windows.Forms.Cursor;

namespace Sidekick.Views
{
    public abstract class BaseWindow : AdonisWindow, ISidekickView
    {
        private readonly IKeybindEvents keybindEvents;
        private readonly SidekickSettings settings;
        private readonly bool closeOnBlur;
        private readonly bool closeOnKey;

        public BaseWindow(IServiceProvider serviceProvider, bool closeOnBlur = false, bool closeOnKey = false)
        {
            keybindEvents = serviceProvider.GetService<IKeybindEvents>();
            settings = serviceProvider.GetService<SidekickSettings>();

            SizeChanged += EnsureBounds;
            IsVisibleChanged += EnsureBounds;
            Loaded += EnsureBounds;

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
        }

        protected bool IsClosing = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            IsClosing = true;
            IsVisibleChanged -= EnsureBounds;
            SizeChanged -= EnsureBounds;
            Loaded -= EnsureBounds;

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

        protected void SetWindowPosition(double x, double y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetWindowPosition(x, y));
                return;
            }

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;
            SetWindowPositionPercent(x / screenRect.Width, y / screenRect.Height);
        }

        protected void SetWindowPositionPercent(double x, double y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => SetWindowPositionPercent(x, y));
                return;
            }

            if (x > 1) { x /= 100; }
            if (y > 1) { y /= 100; }

            var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

            var desiredX = screenRect.X + (screenRect.Width * x);
            var desiredY = screenRect.Y + (screenRect.Height * y);

            Left = (int)desiredX;
            Top = (int)desiredY;
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
                return Dispatcher.Invoke(() => GetWidth());
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
                return Dispatcher.Invoke(() => GetWidth());
            }

            var screen = Screen.FromPoint(MyCursor.Position).Bounds;
            return ActualHeight / screen.Height;
        }

        protected double GetMouseXPercent()
        {
            var screen = Screen.FromPoint(MyCursor.Position).Bounds;

            return (double)(MyCursor.Position.X - screen.X) / screen.Width;
        }

        protected void MoveX(double value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => MoveX(value));
                return;
            }

            Left += value;
        }
    }
}
