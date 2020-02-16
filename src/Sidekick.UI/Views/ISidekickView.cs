using System;

namespace Sidekick.UI.Views
{
    public interface ISidekickView
    {
        void Close();
        event EventHandler Closed;
    }
}
