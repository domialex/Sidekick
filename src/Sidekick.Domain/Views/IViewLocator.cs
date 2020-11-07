namespace Sidekick.Domain.Views
{
    public interface IViewLocator
    {
        void Open(View view);

        void Close(View view);

        bool IsOpened(View view);

        bool IsAnyOpened();

        void CloseAll();
    }
}
