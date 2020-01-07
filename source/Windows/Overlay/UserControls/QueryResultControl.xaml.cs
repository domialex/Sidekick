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
            var uri = TradeClient.POE_TRADE_SEARCH_BASE_URL + TradeClient.SelectedLeague.Id + "/" + e.Uri;
            Process.Start(new ProcessStartInfo(uri));
            e.Handled = true;
        }
    }
}
