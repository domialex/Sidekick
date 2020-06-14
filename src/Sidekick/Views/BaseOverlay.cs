using System;

namespace Sidekick.Views
{
    public abstract class BaseOverlay : BaseWindow, ISidekickView
    {
        public BaseOverlay(string id, IServiceProvider serviceProvider)
            : base(id, serviceProvider, closeOnBlur: true, closeOnKey: true)
        {
        }
    }
}
