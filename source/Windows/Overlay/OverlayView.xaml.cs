using Sidekick.Helpers.POETradeAPI.Models;
using Sidekick.Windows.Overlay.UserControls;
using Sidekick.Windows.Overlay.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Sidekick.Windows.Overlay
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(int width, int height)
        {
            Width = width;
            Height = height;
            InitializeComponent();
            Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public bool IsDisplayed => Visibility == Visibility.Visible;

        public void SetQueryResult(QueryResult<ListingResult> queryResult)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetQueryResultCallback(SetQueryResult), new object[] { queryResult });
            }
            else
            {
                if (!IsDisplayed)
                {
                    return;
                }

                var itemListingControls = queryResult.Result.Select((x, i) => new ListItem(i, new ItemListingControl(x))).ToList();
                DataContext = new
                {
                    queryResult,
                    itemListingControls
                };
            }
        }
        delegate void SetQueryResultCallback(QueryResult<ListingResult> queryResult);

        public void SetWindowPosition(int x, int y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetWindowPositionCallback(SetWindowPosition), new object[] { x, y });
            }
            else
            {
                Left = x;
                Top = y;
            }
        }
        delegate void SetWindowPositionCallback(int x, int y);

        public void ShowWindow()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ShowWindowCallback(ShowWindow));
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }
        delegate void ShowWindowCallback();

        public void HideWindowAndClearData()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HideWindowAndClearDataCallback(HideWindowAndClearData));
            }
            else
            {
                DataContext = null;
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowAndClearDataCallback();
    }
}
