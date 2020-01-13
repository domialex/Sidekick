using Sidekick.Helpers.Localization;
using Sidekick.Helpers.POETradeAPI.Models;
using Sidekick.Windows.Overlay.UserControls;
using Sidekick.Windows.Overlay.ViewModels;
using System;
using System.Collections.Generic;
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
        delegate void SetQueryResultCallback(QueryResult<ListingResult> queryToAppend);

        public void AppendQueryResult(QueryResult<ListingResult> queryToAppend)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new AppendQueryResultCallback(SetQueryResult), new object[] { queryToAppend });
            }
            else
            {
                if (!IsDisplayed)
                {
                    return;
                }

                // TODO: Get this to work, so infinite scroll works
                //ReadDataContext(out QueryResult<ListingResult> queryResult, out List<ListItem> itemListingControls);

                ////append newly fetched result to query and refresh total count
                //queryResult.Total = queryResult.Total;
                //queryResult.Result.AddRange(queryResult.Result);

                ////append newly fetched items
                //itemListingControls.AddRange(queryToAppend.Result.Select((x, i) => new ListItem(i, new ItemListingControl(x))).ToList());

                //DataContext = new
                //{
                //    queryResult,
                //    itemListingControls
                //};
            }
        }
        delegate void AppendQueryResultCallback(QueryResult<ListingResult> queryResult);

        /// <summary>
        /// Used to cast anonymous types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeHolder"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private T Cast<T>(T typeHolder, object x)
        {
            return (T)x;
        }

        

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
                _itemList.ScrollToTop();
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

        public delegate void ItemScrollReachedEndHandler(Helpers.Item item, int displayedItemsCount);
        public event ItemScrollReachedEndHandler ItemScrollReachedEnd;
        public void OnItemScrollReachedEnd(Helpers.Item item, int displayedItemsCount)
        {
            ItemScrollReachedEnd?.Invoke(item, displayedItemsCount);
        }

        private void _itemList_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            //Load next results when scrollviewer is at the bottom
            if(_itemList.VerticalOffset == _itemList.ScrollableHeight && _itemList.ScrollableHeight > 0)
            {
                ReadDataContext(out QueryResult<ListingResult> queryResult, out List<ListItem> items);
                OnItemScrollReachedEnd(queryResult.Item, items.Count);
            }
        }

        private bool ReadDataContext(out QueryResult<ListingResult> query, out List<ListItem> items)
        {
            try
            {
                items = (List<ListItem>)DataContext?.GetType().GetProperty("itemListingControls")?.GetValue(DataContext, null);
                query = (QueryResult<ListingResult>)DataContext?.GetType().GetProperty("queryResult")?.GetValue(DataContext, null);
                return true;
            }
            catch (Exception)
            {
                throw;
            }           
        }
    }
}
