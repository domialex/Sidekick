using System;

namespace Sidekick.Views
{
    public interface ISidekickView
    {
        bool IsVisible { get; }
        void Close();
        void Hide();
        void Show();
        event EventHandler Closed;
    }
}
