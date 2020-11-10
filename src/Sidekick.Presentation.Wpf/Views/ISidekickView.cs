using System;
using System.Threading.Tasks;

namespace Sidekick.Presentation.Wpf.Views
{
    public interface ISidekickView
  {
    Task Open(params object[] args);
    void Close();
    void Hide();
    event EventHandler Closed;
  }
}
