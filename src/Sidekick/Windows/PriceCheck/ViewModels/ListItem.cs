namespace Sidekick.Windows.PriceCheck.ViewModels
{
    public class ListItem
    {
        public ListItem(int index, object item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; set; }
        public bool Odd => Index % 2 != 0;
        public object Item { get; set; }
    }
}
