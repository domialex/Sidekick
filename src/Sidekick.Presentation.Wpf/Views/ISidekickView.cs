using System;
using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Wpf.Views
{
    public interface ISidekickView
    {
        View View { get; }
        Task Open(params object[] args);
        bool IsVisible { get; }
        void Hide();
        void Close();
        event EventHandler Closed;
    }
}
