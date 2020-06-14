using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;

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

            var text = filter.Item.Text;
            var matches = Highlight.Matches(text);
            var highlightMatches = new Dictionary<int, string>();

            for (var i = 0; i < matches.Count; i++)
            {
                var info = matches[i];

                highlightMatches.Add(info.Index, info.Value);
            }

            filter.TextBlock.Inlines.Clear();
            var index = 0;
            while (text.Length > 0)
            {
                if (highlightMatches.ContainsKey(index))
                {
                    filter.TextBlock.Inlines.Add(new Run(highlightMatches[index])
                    {
                        Foreground = Brushes.LightBlue,
                        FontWeight = FontWeight.FromOpenTypeWeight(700),
                    });
                    text = text.Substring(highlightMatches[index].Length);
                    index += highlightMatches[index].Length;
                    continue;
                }

                var nextIndex = highlightMatches.Keys.Where(x => x > index).OrderByDescending(x => x).FirstOrDefault();
                filter.TextBlock.Inlines.Add(text.Substring(0, nextIndex == default ? text.Length : nextIndex - index));
                text = text.Substring(nextIndex == default ? text.Length : nextIndex - index);
                index += nextIndex - index;
            }

            //filter.RichText.Document.Blocks.Clear();
            //filter.RichText.Document.Blocks.Add(new Paragraph(new Run(filter.Item.Text)));
            //filter.RichText.Foreground = filter.Item.Type == nameof(StatFilter) ? Brushes.White : Brushes.LightGray;

            //var matches = Highlight.Matches(filter.Item.Text);

            //// create textpointer translator
            //var trans = new TextPointerTranslator(filter.RichText.Document);

            //// enumerate
            //for (var i = 0; i < matches.Count; i++)
            //{
            //    var info = matches[i];
            //    var start = trans.GetTextPointer(info.Index, false);
            //    var end = trans.GetTextPointer(info.Index + info.Value.Length, false);

            //    if (start == null || end == null)
            //    {
            //        continue;
            //    }

            //    var range = new TextRange(start, end);

            //    if (range != null)
            //    {
            //        range.ApplyPropertyValue(
            //           TextElement.ForegroundProperty, Brushes.LightBlue);
            //        range.ApplyPropertyValue(
            //           TextElement.FontWeightProperty, FontWeight.FromOpenTypeWeight(700));
            //    }
            //}
        }
    }
}
