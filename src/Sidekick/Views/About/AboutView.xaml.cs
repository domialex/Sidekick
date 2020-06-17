using System;

namespace Sidekick.Views.About
{
    public partial class AboutView : BaseWindow, ISidekickView
    {
        public AboutView(IServiceProvider serviceProvider)
            : base("about", serviceProvider)
        {
            InitializeComponent();

            Show();
        }
    }
}
