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
    public partial class ItemLineContent : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d,\\.]+[%]?");

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

            itemProperty.RichText.Document.Blocks.Clear();
            itemProperty.RichText.Document.Blocks.Add(new Paragraph(new Run(itemProperty.Property.Parsed)));

            // create textpointer translator
            var trans = new TextPointerTranslator(itemProperty.RichText.Document);

            // enumerate
            foreach (var value in itemProperty.Property.Values)
            {
                var index = itemProperty.Property.Parsed.IndexOf(value.Value);
                var start = trans.GetTextPointer(index, false);
                var end = trans.GetTextPointer(index + value.Value.Length, false);

                if (start == null || end == null)
                {
                    continue;
                }

                var range = new TextRange(start, end);
                if (range != null)
                {
                    range.ApplyPropertyValue(
                       TextElement.FontWeightProperty, FontWeight.FromOpenTypeWeight(700));

                    switch (value.Type)
                    {
                        case LineContentType.Simple:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.White);
                            break;
                        case LineContentType.Augmented:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.LightBlue);
                            break;
                        case LineContentType.Cold:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.LightSkyBlue);
                            break;
                        case LineContentType.Fire:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.DeepPink);
                            break;
                        case LineContentType.Lightning:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.LightYellow);
                            break;
                        case LineContentType.Chaos:
                            range.ApplyPropertyValue(
                               TextElement.ForegroundProperty, Brushes.Purple);
                            break;
                    }
                }
            }
        }
    }
}
