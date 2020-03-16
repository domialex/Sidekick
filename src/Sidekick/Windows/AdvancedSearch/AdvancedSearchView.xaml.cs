using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;
using Sidekick.UI.Items;

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

        private TextBox ItemLevelTextBox;
        private CheckBox InfluenceCheckBox;
        private CheckBox SocketCheckBox;
        private CheckBox LinkCheckBox;
        private CheckBox CorruptedCheckBox;

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
            ItemLevelTextBox = null;
            InfluenceCheckBox = null;
            SocketCheckBox = null;
            LinkCheckBox = null;
        }

        public void PopulateGrid(Item item)
        {
            if ((item as IAttributeItem) == null)
            {
                return;
            }

            ClearGrid();
            var attributeItem = (IAttributeItem)item;
            CurrentItem = item;
            int rowCounter = 0;
            RowElementsDictionary = new Dictionary<int, (TextBlock attrName, TextBox minVal, TextBox maxVal, CheckBox isChecked)>();

            // Item Name and Type
            gridAdvancedSearch.RowDefinitions.Add(BuildRow());
            var itemNameBlock = BuildTextBlock(item.Name);
            var itemTypeBlock = BuildTextBlock(item.Type);
            itemNameBlock.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(item.GetColor());
            itemTypeBlock.Margin = new Thickness(150, 0, 0, 0);
            gridAdvancedSearch.Children.Add(itemNameBlock);
            gridAdvancedSearch.Children.Add(itemTypeBlock);
            Grid.SetRow(itemNameBlock, rowCounter);
            Grid.SetColumn(itemNameBlock, 0);
            Grid.SetRow(itemTypeBlock, rowCounter);
            Grid.SetColumn(itemTypeBlock, 1);
            rowCounter++;

            // Item Attributes
            foreach (var pair in attributeItem.AttributeDictionary)
            {
                gridAdvancedSearch.RowDefinitions.Add(BuildRow());
                var attributeBlock = BuildTextBlock(pair.Key.Text);
                var minBlock = BuildTextBox(pair.Value.Min == null ? "" : pair.Value.Min.ToString());
                minBlock.IsEnabled = false;
                var maxBlock = BuildTextBox(pair.Value.Max == null ? "" : pair.Value.Max.ToString());
                maxBlock.IsEnabled = false;
                var enabledCheckBox = new CheckBox() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, IsChecked = false };
                enabledCheckBox.Checked += EnableTextBoxes;
                enabledCheckBox.Unchecked += DisableTextBoxes;
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

            // Links + Sockets + Item Level + Influence + Corruption
            if (item.GetType().IsSubclassOf(typeof(EquippableItem)))
            {
                gridAdvancedSearch.RowDefinitions.Add(BuildRow());
                var equippableItem = (EquippableItem)item;
                SocketCheckBox = new CheckBox() { Content = $"Max Sockets ({equippableItem.MaxSockets})" };
                LinkCheckBox = new CheckBox() { Content = $"Max Links ({equippableItem.MaxSockets})", Margin = new Thickness(110, 0, 0, 0) };
                ItemLevelTextBox = new TextBox() { Text = equippableItem.ItemLevel?.ToString(), Margin = new Thickness(300, 0, 0, 0), Width = 35, Height = 25, IsEnabled = false, MaxLength = 3 };
                var itemLevelCheckBox = new CheckBox() { Content = "Minimum ILvl", Margin = new Thickness(210, 0, 0, 0) };
                itemLevelCheckBox.Checked += (s, args) => { ItemLevelTextBox.IsEnabled = true; };
                itemLevelCheckBox.Unchecked += (s, args) => { ItemLevelTextBox.IsEnabled = false; };

                if (equippableItem.Influence != InfluenceType.None)
                {
                    InfluenceCheckBox = new CheckBox() { Content = equippableItem.Influence.ToString() };
                    gridAdvancedSearch.Children.Add(InfluenceCheckBox);
                    Grid.SetRow(InfluenceCheckBox, rowCounter);
                    Grid.SetColumn(InfluenceCheckBox, 2);
                }

                if (equippableItem.IsCorrupted)
                {
                    // TODO If corrupted show checkbox
                }

                gridAdvancedSearch.Children.Add(SocketCheckBox);
                gridAdvancedSearch.Children.Add(LinkCheckBox);
                gridAdvancedSearch.Children.Add(itemLevelCheckBox);
                gridAdvancedSearch.Children.Add(ItemLevelTextBox);

                Grid.SetRow(SocketCheckBox, rowCounter);
                Grid.SetColumn(SocketCheckBox, 0);
                Grid.SetRow(LinkCheckBox, rowCounter);
                Grid.SetColumn(LinkCheckBox, 0);
                Grid.SetRow(itemLevelCheckBox, rowCounter);
                Grid.SetColumn(itemLevelCheckBox, 0);
                Grid.SetRow(ItemLevelTextBox, rowCounter);
                Grid.SetColumn(ItemLevelTextBox, 0);
                rowCounter++;
            }
            else if (item.GetType() == typeof(MapItem))
            {
                // TODO
            }

            // Search Button
            gridAdvancedSearch.RowDefinitions.Add(BuildRow(50));
            var searchButton = new Button() { Content = "Search", Height = 30, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            searchButton.Click += Search_Click;

            Grid.SetColumn(searchButton, MinValueColumnIndex);
            Grid.SetRow(searchButton, rowCounter);
            gridAdvancedSearch.Children.Add(searchButton);
            gridAdvancedSearch.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            gridAdvancedSearch.MinHeight += 10;
            gridAdvancedSearch.MinWidth += 10;
            gridAdvancedSearch.UpdateLayout();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            var choosenAttributesDict = new Dictionary<StatData, FilterValue>();

            foreach (var pair in RowElementsDictionary)
            {
                if (pair.Value.isChecked.IsChecked == true)
                {
                    var attr = GetAttribute(pair.Value.attrName.Text);
                    var value = GetValue(pair.Key);
                    choosenAttributesDict.Add(attr, value);
                }
            }

            ((IAttributeItem)CurrentItem).AttributeDictionary = choosenAttributesDict;

            if (ItemLevelTextBox != null && ItemLevelTextBox.IsEnabled)
            {
                if (int.TryParse(ItemLevelTextBox.Text, out _))
                {
                    ((EquippableItem)CurrentItem).ItemLevel = ItemLevelTextBox.Text;
                }
                else
                {
                    ((EquippableItem)CurrentItem).ItemLevel = "0";
                }
            }

            if (InfluenceCheckBox != null && InfluenceCheckBox.IsChecked == true)
            {
                if (Enum.TryParse<InfluenceType>(InfluenceCheckBox.Content.ToString(), out var influence))
                {
                    ((EquippableItem)CurrentItem).Influence = influence;
                }
                else
                {
                    ((EquippableItem)CurrentItem).Influence = InfluenceType.None;
                }
            }

            // TODO Better socket search (Custom Colors, Links, etc.)
            if (SocketCheckBox != null && SocketCheckBox.IsChecked == true)
            {
                ((EquippableItem)CurrentItem).Sockets = new SocketFilterOption() { Min = ((EquippableItem)CurrentItem).MaxSockets };
            }

            if (LinkCheckBox != null && LinkCheckBox.IsChecked == true)
            {
                ((EquippableItem)CurrentItem).Links = new SocketFilterOption() { Min = ((EquippableItem)CurrentItem).MaxSockets };
            }

            Controller.CheckItemPrice(CurrentItem);
        }

        private void DisableTextBoxes(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            var gridRow = Grid.GetRow(checkbox);
            EnableDisableTextBoxes(gridRow, false);
        }

        private void EnableTextBoxes(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            var gridRow = Grid.GetRow(checkbox);
            EnableDisableTextBoxes(gridRow, true);
        }

        private void EnableDisableTextBoxes(int row, bool toggle)
        {
            RowElementsDictionary[row].minVal.IsEnabled = toggle;
            RowElementsDictionary[row].maxVal.IsEnabled = toggle;
        }

        private StatData GetAttribute(string text)
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

            if (int.TryParse(minValStr, out var val))
            {
                minVal = val;
            }

            if (int.TryParse(maxValStr, out val))
            {
                maxVal = val;
            }

            return new FilterValue() { Min = minVal, Max = maxVal };
        }

        private RowDefinition BuildRow(int? height = null)
        {
            if (height == null)
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
                HorizontalAlignment = HorizontalAlignment.Left,
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
                ClearGrid();
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowAndClearDataCallback();

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
    }
}
