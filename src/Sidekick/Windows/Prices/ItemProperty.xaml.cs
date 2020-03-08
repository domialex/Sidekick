using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;
using Sidekick.Business.Trades.Results;
using Sidekick.Windows.Prices.Helpers;

namespace Sidekick.Windows.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemProperty : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d,\\.]+[%]?");

        [DependencyProperty(OnPropertyChanged = nameof(OnPropertyChanged))]
        public Property Property { get; set; }

        public ItemProperty()
        {
            InitializeComponent();
        }

        public static void OnPropertyChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
        {
            var itemProperty = (ItemProperty)dependencyObject;

            itemProperty.RichText.Document.Blocks.Clear();
            itemProperty.RichText.Document.Blocks.Add(new Paragraph(new Run(itemProperty.Property.Parsed)));

            var matches = Highlight.Matches(itemProperty.Property.Parsed);

            // create textpointer translator
            var trans = new TextPointerTranslator(itemProperty.RichText.Document);

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
                       TextElement.ForegroundProperty, Brushes.LightGreen);
                    range.ApplyPropertyValue(
                       TextElement.FontWeightProperty, FontWeight.FromOpenTypeWeight(700));
                }
            }
        }
    }
}
