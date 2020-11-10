using System;

namespace Sidekick.Presentation.Wpf.Views
{
    public abstract class BaseOverlay : BaseView
  {
    protected BaseOverlay()
        : base()
    {
      // An empty constructor is necessary for the designer to show a preview
    }

    protected BaseOverlay(string id, IServiceProvider serviceProvider)
        : base(id, serviceProvider, closeOnBlur: true)
    {
    }
  }
}
