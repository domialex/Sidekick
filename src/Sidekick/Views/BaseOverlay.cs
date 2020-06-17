using System;

namespace Sidekick.Views
{
    public abstract class BaseOverlay : BaseWindow, ISidekickView
    {
        public BaseOverlay()
        {
            // An empty constructor is necessary for the designer to show a preview
        }

        public BaseOverlay(string id, IServiceProvider serviceProvider)
            : base(id, serviceProvider, closeOnBlur: true, closeOnKey: true)
        {
        }
    }
}
