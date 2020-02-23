using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Natives;
using Sidekick.UI.Views;
using MyCursor = System.Windows.Forms.Cursor;

namespace Sidekick.Windows
{
    public abstract class BaseWindow : Window, ISidekickView
    {
        private readonly IKeybindEvents keybindEvents;

        public BaseWindow(IServiceProvider serviceProvider)
        {
            keybindEvents = serviceProvider.GetService<IKeybindEvents>();

            MouseLeftButtonDown += Window_MouseLeftButtonDown;
            keybindEvents.OnCloseWindow += KeybindEvents_OnCloseWindow;
        }

        private Task<bool> KeybindEvents_OnCloseWindow()
        {
            keybindEvents.OnCloseWindow -= KeybindEvents_OnCloseWindow;
            Close();
            return Task.FromResult(true);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public new void Show()
        {
            base.Show();
            EnsureBounds();
        }

        protected void SetWindowPositionPercent(double x, double y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetWindowPositionPercentCallback(EnsureBounds), x, y);
            }
            else
            {
                if (x > 1) { x /= 100; }
                if (y > 1) { y /= 100; }

                var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

                var desiredX = screenRect.X + (screenRect.Width * x);
                var desiredY = screenRect.Y + (screenRect.Height * y);

                Left = (int)desiredX;
                Top = (int)desiredY;
                EnsureBounds();
            }
        }
        private delegate void SetWindowPositionPercentCallback();

        protected void EnsureBounds()
        {
            if (IsVisible)
            {
                var screenRect = Screen.FromPoint(MyCursor.Position).Bounds;

                if (Left + Width > screenRect.X + screenRect.Width)
                {
                    Left = screenRect.X + screenRect.Width - Width;
                }
                if (Top + Height > screenRect.Y + screenRect.Height)
                {
                    Top = screenRect.Y + screenRect.Height - Height;
                }
            }
        }
    }
}
