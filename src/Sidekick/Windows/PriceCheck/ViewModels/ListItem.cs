namespace Sidekick.Windows.PriceCheck.ViewModels
{
    public class ListItem
    {
        public ListItem(int index, object item)
        {
            Index = index;
            Item = item;
            Odd = Index % 2 != 0;
        }
        public int Index { get; set; }
        public bool Odd { get; set; }
        public object Item { get; set; }
    }
}
