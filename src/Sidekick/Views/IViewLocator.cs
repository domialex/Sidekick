namespace Sidekick.Views
{
    public interface IViewLocator
    {
        void Open<TView>()
            where TView : ISidekickView;

        bool IsOpened<TView>();

        void CloseAll();
    }
}
