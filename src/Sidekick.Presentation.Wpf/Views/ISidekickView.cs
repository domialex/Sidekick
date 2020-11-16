using System;
using System.Threading.Tasks;

namespace Sidekick.Presentation.Wpf.Views
{
    public interface ISidekickView
    {
        Task Open(params object[] args);
        bool IsVisible { get; }
        void Hide();
        void Close();
        event EventHandler Closed;
    }
}
