namespace Sidekick.UI.Views
{
    public interface IViewController
    {
        void Open<TView>()
            where TView : ISidekickView;
    }
}
