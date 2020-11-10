using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bindables;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Views.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemMod : UserControl
    {
        private static readonly Regex Highlight = new Regex("[\\+]?[\\d]+(?:[,\\.]\\d+)?[%]?");

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

            var text = itemMod.Modifier.Text;
            var matches = Highlight.Matches(text);
            var highlightMatches = new Dictionary<int, string>();

            for (var i = 0; i < matches.Count; i++)
            {
                var info = matches[i];

                highlightMatches.Add(info.Index, info.Value);
            }

            itemMod.TextBlock.Inlines.Clear();
            var index = 0;
            while (text.Length > 0)
            {
                if (highlightMatches.ContainsKey(index))
                {
                    itemMod.TextBlock.Inlines.Add(new Run(highlightMatches[index])
                    {
                        Foreground = Brushes.LightBlue,
                        FontWeight = FontWeight.FromOpenTypeWeight(700),
                    });
                    text = text.Substring(highlightMatches[index].Length);
                    index += highlightMatches[index].Length;
                    continue;
                }

                var nextIndex = highlightMatches.Keys.Where(x => x > index).OrderBy(x => x).FirstOrDefault();
                itemMod.TextBlock.Inlines.Add(text.Substring(0, nextIndex == default ? text.Length : nextIndex - index));
                text = text.Substring(nextIndex == default ? text.Length : nextIndex - index);
                index += nextIndex - index;
            }
        }
    }
}
