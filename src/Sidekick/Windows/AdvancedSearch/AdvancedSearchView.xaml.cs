using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sidekick.Business.Filters;
using Sidekick.Business.Parsers.Models;
using Sidekick.Windows.AdvancedSearch.ViewModels;
using Sidekick.Windows.PriceCheck;

namespace Sidekick.Windows.AdvancedSearch
{
    /// <summary>
    /// Interaction logic for AdvancedSearchView.xaml
    /// </summary>
    public partial class AdvancedSearchView : Window
    {
        private const int AttributeColumnIndex = 0;
        private const int MinValueColumnIndex = 1;
        private const int MaxValueColumnIndex = 2;
        private const int EnabledColumnIndex = 3;

        public Item CurrentItem;
        private AdvancedSearchController Controller;
        private Dictionary<int, (TextBlock attrName, TextBox minVal, TextBox maxVal, CheckBox isChecked)> RowElementsDictionary;

        public AdvancedSearchView(AdvancedSearchController controller)
        {
            InitializeComponent();
            Controller = controller;
            RowElementsDictionary = new Dictionary<int, (TextBlock attrName, TextBox minVal, TextBox maxVal, CheckBox isChecked)>();
            ClearGrid();
            Hide();
        }

        public void ClearGrid()
        {
            gridAdvancedSearch.Children.Clear();
            gridAdvancedSearch.RowDefinitions.Clear();
            RowElementsDictionary.Clear();
            CurrentItem = null;
        }

        public void PopulateGrid(Item item)
        {
            if((item as IAttributeItem) == null)
            {
                return;
            }

            ClearGrid();
            var attributeItem = (IAttributeItem)item;
            CurrentItem = item;
            int rowCounter = 0;
            RowElementsDictionary = new Dictionary<int, (TextBlock attrName, TextBox minVal, TextBox maxVal, CheckBox isChecked)>();

            foreach(var pair in attributeItem.AttributeDictionary)
            {
                gridAdvancedSearch.RowDefinitions.Add(BuildRow());
                var attributeBlock = BuildTextBlock(pair.Key.Text);
                var minBlock = BuildTextBox(pair.Value.Min == null ? "" : pair.Value.Min.ToString());
                var maxBlock = BuildTextBox(pair.Value.Max == null ? "" : pair.Value.Max.ToString());
                var enabledCheckBox = new CheckBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsChecked = true };
                Grid.SetColumn(attributeBlock, AttributeColumnIndex);
                Grid.SetRow(attributeBlock, rowCounter);
                Grid.SetColumn(minBlock, MinValueColumnIndex);
                Grid.SetRow(minBlock, rowCounter);
                Grid.SetColumn(maxBlock, MaxValueColumnIndex);
                Grid.SetRow(maxBlock, rowCounter);
                Grid.SetColumn(enabledCheckBox, EnabledColumnIndex);
                Grid.SetRow(enabledCheckBox, rowCounter);
                gridAdvancedSearch.Children.Add(attributeBlock);
                gridAdvancedSearch.Children.Add(minBlock);
                gridAdvancedSearch.Children.Add(maxBlock);
                gridAdvancedSearch.Children.Add(enabledCheckBox);
                RowElementsDictionary.Add(rowCounter, (attributeBlock, minBlock, maxBlock, enabledCheckBox));
                rowCounter++;
            }

            gridAdvancedSearch.RowDefinitions.Add(BuildRow(50));
            var searchButton = new Button() { Content = "Search", Height = 30, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            searchButton.Click += Search_Click;

            Grid.SetColumn(searchButton, MinValueColumnIndex);
            Grid.SetRow(searchButton, rowCounter);
            gridAdvancedSearch.Children.Add(searchButton);
            gridAdvancedSearch.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            gridAdvancedSearch.UpdateLayout();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            var choosenAttributesDict = new Dictionary<Business.Apis.Poe.Models.Attribute, Business.Filters.FilterValue>();

            foreach(var pair in RowElementsDictionary)
            {
                if(pair.Value.isChecked.IsChecked == true)
                {
                    var attr = GetAttribute(pair.Value.attrName.Text);
                    var value = GetValue(pair.Key);
                    choosenAttributesDict.Add(attr, value);
                }
            }

            ((IAttributeItem)CurrentItem).AttributeDictionary = choosenAttributesDict;
            Controller.CheckItemPrice(CurrentItem);
        }

        private Business.Apis.Poe.Models.Attribute GetAttribute(string text)
        {
            var attr = ((IAttributeItem)CurrentItem).AttributeDictionary;
            var entry = attr.Where(c => c.Key.Text == text).FirstOrDefault().Key;

            return entry;
        }

        private FilterValue GetValue(int row)
        {
            var minValStr = RowElementsDictionary[row].minVal.Text;
            var maxValStr = RowElementsDictionary[row].maxVal.Text;
            int? minVal = null;
            int? maxVal = null;

            if(int.TryParse(minValStr, out var val))
            {
                minVal = val;
            }

            if(int.TryParse(maxValStr, out val))
            {
                maxVal = val;
            }

            return new FilterValue() { Min = minVal, Max = maxVal };
        }

        private RowDefinition BuildRow(int? height = null)
        {
            if(height == null)
            {
                height = 30;
            }

            return new RowDefinition()
            {
                Height = new GridLength(height.Value, GridUnitType.Pixel),
                MinHeight = height.Value,
                MaxHeight = height.Value,
            };
        }

        private TextBlock BuildTextBlock(string text)
        {
            return new TextBlock()
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }

        private TextBox BuildTextBox(string value)
        {
            return new TextBox()
            {
                Text = value,
                Height = 30,
                Width = 60,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
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

        public void SetWindowPosition(int x, int y)
        {
            if(!Dispatcher.CheckAccess())
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
    }
}
