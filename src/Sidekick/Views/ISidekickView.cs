using System;

namespace Sidekick.Views
{
    public interface ISidekickView
    {
        void Close();
        event EventHandler Closed;
    }
}
