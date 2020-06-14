using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;

namespace Sidekick.Views.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemLineContent : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d]+(?:[,\\.]\\d+)?[%]?");

        [DependencyProperty(OnPropertyChanged = nameof(OnPropertyChanged))]
        public LineContent Property { get; set; }

        public ItemLineContent()
        {
            InitializeComponent();
        }

        public static void OnPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            var itemProperty = (ItemLineContent)dependencyObject;

            var highlightMatches = new Dictionary<int, (string Value, LineContentType Type)>();
            var text = itemProperty.Property.Parsed;

            foreach (var value in itemProperty.Property.Values)
            {
                var matches = Highlight.Matches(value.Value);
                var offset = text.IndexOf(value.Value);

                for (var i = 0; i < matches.Count; i++)
                {
                    var info = matches[i];

                    highlightMatches.Add(offset + info.Index, (info.Value, value.Type));
                }
            }

            itemProperty.TextBlock.Inlines.Clear();
            var index = 0;
            while (text.Length > 0)
            {
                if (highlightMatches.ContainsKey(index))
                {
                    var (Value, Type) = highlightMatches[index];
                    var run = new Run(highlightMatches[index].Value)
                    {
                        Foreground = Brushes.LightBlue,
                        FontWeight = FontWeight.FromOpenTypeWeight(700),
                    };
                    switch (Type)
                    {
                        case LineContentType.Simple:
                            run.Foreground = Brushes.White;
                            break;
                        case LineContentType.Augmented:
                            run.Foreground = Brushes.LightBlue;
                            break;
                    }
                    itemProperty.TextBlock.Inlines.Add(run);
                    text = text.Substring(highlightMatches[index].Value.Length);
                    index += highlightMatches[index].Value.Length;
                    continue;
                }

                var nextIndex = highlightMatches.Keys.Where(x => x > index).OrderBy(x => x).FirstOrDefault();
                itemProperty.TextBlock.Inlines.Add(text.Substring(0, nextIndex == default ? text.Length : nextIndex - index));
                text = text.Substring(nextIndex == default ? text.Length : nextIndex - index);
                index += nextIndex - index;
            }
        }
    }
}
