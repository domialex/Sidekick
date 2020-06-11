using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using AdonisUI.Controls;
using MyCursor = System.Windows.Forms.Cursor;

namespace Sidekick.Views
{
    public abstract class BaseTitleWindow : AdonisWindow, ISidekickView
    {
        public BaseTitleWindow()
        {
            SizeChanged += EnsureBounds;
            IsVisibleChanged += EnsureBounds;
            Loaded += EnsureBounds;
        }

        protected bool IsClosing = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsClosing) return;

            IsClosing = true;
            IsVisibleChanged -= EnsureBounds;
            SizeChanged -= EnsureBounds;
            Loaded -= EnsureBounds;

            base.OnClosing(e);
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

        protected double GetHeight()
        {
            if (!Dispatcher.CheckAccess())
            {
                return Dispatcher.Invoke(() => GetHeight());
            }

            return ActualHeight;
        }
    }
}
