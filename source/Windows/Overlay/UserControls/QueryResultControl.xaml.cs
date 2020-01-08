using Sidekick.Helpers;
using Sidekick.Helpers.POETradeAPI;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sidekick.Windows.Overlay.UserControls
{
    public partial class QueryResultControl : UserControl
    {
        public QueryResultControl()
        {
            InitializeComponent();
        }

        private void openQueryLink(object sender, RequestNavigateEventArgs e)
        {
            string uri = e.Uri.ToString();
            Logger.Log(string.Format("Opening in browser: {0}", uri));
            Process.Start(new ProcessStartInfo(uri));
            e.Handled = true;
        }
    }
}
