using System;

namespace Sidekick.Views
{
    public abstract class BaseOverlay : BaseWindow, ISidekickView
    {
        public BaseOverlay(IServiceProvider serviceProvider)
            : base(serviceProvider, closeOnBlur: true, closeOnKey: true)
        {
        }
    }
}
