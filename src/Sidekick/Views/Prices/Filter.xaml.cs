using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;
using Sidekick.Business.Apis.Poe.Trade.Search.Filters;
using Sidekick.Views.Prices.Helpers;

namespace Sidekick.Views.Prices
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Filter : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d,\\.]+[%]?");

        [DependencyProperty(OnPropertyChanged = nameof(OnItemChanged))]
        public PriceFilter Item { get; set; }

        public Filter()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public static void OnItemChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            var filter = (Filter)dependencyObject;

            filter.RichText.Document.Blocks.Clear();
            filter.RichText.Document.Blocks.Add(new Paragraph(new Run(filter.Item.Text)));
            filter.RichText.Foreground = filter.Item.Type == nameof(StatFilter) ? Brushes.White : Brushes.LightGray;

            var matches = Highlight.Matches(filter.Item.Text);

            // create textpointer translator
            var trans = new TextPointerTranslator(filter.RichText.Document);

            // enumerate
            for (var i = 0; i < matches.Count; i++)
            {
                var info = matches[i];
                var start = trans.GetTextPointer(info.Index, false);
                var end = trans.GetTextPointer(info.Index + info.Value.Length, false);

                if (start == null || end == null)
                {
                    continue;
                }

                var range = new TextRange(start, end);

                if (range != null)
                {
                    range.ApplyPropertyValue(
                       TextElement.ForegroundProperty, Brushes.LightBlue);
                    range.ApplyPropertyValue(
                       TextElement.FontWeightProperty, FontWeight.FromOpenTypeWeight(700));
                }
            }
        }
    }
}
