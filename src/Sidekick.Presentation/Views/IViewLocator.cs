namespace Sidekick.Presentation.Views
{
    public interface IViewLocator
    {
        void Open(View view);

        void Close(View view);

        bool IsOpened(View view);

        void CloseAll();
    }
}
