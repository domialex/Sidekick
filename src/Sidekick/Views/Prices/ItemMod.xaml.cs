using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Views.Prices.Helpers;

namespace Sidekick.Views.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemMod : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d,\\.]+[%]?");

        [DependencyProperty(OnPropertyChanged = nameof(OnTextChanged))]
        public Modifier Modifier { get; set; }

        public ItemMod()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public static void OnTextChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
        {
            var itemMod = (ItemMod)dependencyObject;

            itemMod.RichText.Document.Blocks.Clear();
            itemMod.RichText.Document.Blocks.Add(new Paragraph(new Run(itemMod.Modifier.Text)));

            var matches = Highlight.Matches(itemMod.Modifier.Text);

            // create textpointer translator
            var trans = new TextPointerTranslator(itemMod.RichText.Document);

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
