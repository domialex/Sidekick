using System;

namespace Sidekick.Views
{
    public interface ISidekickView
    {
        void Close();
        void Show();
        event EventHandler Closed;
    }
}
