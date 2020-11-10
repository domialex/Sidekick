using System;
using System.Threading.Tasks;

namespace Sidekick.Views
{
    public interface ISidekickView
    {
        Task Open(params object[] args);
        void Close();
        void Hide();
        event EventHandler Closed;
    }
}
