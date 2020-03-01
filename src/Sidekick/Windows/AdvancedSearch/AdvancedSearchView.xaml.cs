using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Windows.AdvancedSearch
{
    /// <summary>
    /// Interaction logic for AdvancedSearchView.xaml
    /// </summary>
    public partial class AdvancedSearchView : Window
    {
        private Dictionary<int, TextBlock> AttributeNameBlockDicitonary;

        public AdvancedSearchView()
        {
            InitializeComponent();
            AttributeNameBlockDicitonary = new Dictionary<int, TextBlock>()
            {
                { 0, textBlockAttr1 },
                { 1, textBlockAttr2 },
                { 2, textBlockAttr3 },
                { 3, textBlockAttr4 },
                { 4, textBlockAttr5 },
                { 5, textBlockAttr6 },
                { 6, textBlockAttr7 },
                { 7, textBlockAttr8 },
                { 8, textBlockAttr9 },
                { 9, textBlockAttr10 },
                { 10, textBlockAttr11 },
                { 11, textBlockAttr12 },
            };
            ClearGrid();
            Hide();
        }

        public void ClearGrid()
        {
            foreach(var pair in AttributeNameBlockDicitonary)
            {
                pair.Value.Text = "";
                pair.Value.IsEnabled = false;
            }

            gridAdvancedSearch.RowDefinitions.All(c => { c.Height = new GridLength(0, GridUnitType.Pixel); return true; });
            gridAdvancedSearch.UpdateLayout();
        }

        public void PopulateGrid(Business.Parsers.Models.Item item)
        {
            ClearGrid();

            if((item as IAttributeItem) == null)
            {
                return;
            }

            var attributeItem = (IAttributeItem)item;
            int counter = 0;

            foreach(var pair in attributeItem.AttributeDictionary)
            {
                AttributeNameBlockDicitonary[counter].Text = pair.Key.Text;
                AttributeNameBlockDicitonary[counter].IsEnabled = true;
                gridAdvancedSearch.RowDefinitions[counter].Height = new GridLength(30, GridUnitType.Pixel);
                counter++;
            }

            gridAdvancedSearch.UpdateLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public void ShowWindow()
        {
            if(!Dispatcher.CheckAccess())
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
            if(!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HideWindowAndClearDataCallback(HideWindowAndClearData));
            }
            else
            {
                ClearGrid();
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowAndClearDataCallback();
    }
}
