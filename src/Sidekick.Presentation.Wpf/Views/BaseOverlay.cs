using System;
using System.ComponentModel;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Wpf.Views
{
    public abstract class BaseOverlay : BaseView
    {
        protected BaseOverlay()
            : base()
        {
            // An empty constructor is necessary for the designer to show a preview
        }

        protected BaseOverlay(View id, IServiceProvider serviceProvider)
            : base(id, serviceProvider)
        {
            Topmost = true;

            if ((id == View.Price && settings.Price_CloseWithMouse)
                || (id == View.Map && settings.Map_CloseWithMouse))
            {
                Deactivated += BaseBorderlessWindow_Deactivated;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Deactivated -= BaseBorderlessWindow_Deactivated;

            base.OnClosing(e);
        }

        private void BaseBorderlessWindow_Deactivated(object sender, EventArgs e)
        {
            Close();
        }
    }
}
