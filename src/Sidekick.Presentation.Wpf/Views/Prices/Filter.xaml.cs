using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Filter : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d]+(?:[,\\.]\\d+)?[%]?");

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

                var nextIndex = highlightMatches.Keys.Where(x => x > index).OrderBy(x => x).FirstOrDefault();
                filter.TextBlock.Inlines.Add(text.Substring(0, nextIndex == default ? text.Length : nextIndex - index));
                text = text.Substring(nextIndex == default ? text.Length : nextIndex - index);
                index += nextIndex - index;
            }
        }
    }
}
