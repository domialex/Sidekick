namespace Sidekick.Domain.Views
{
    public interface IViewLocator
    {
        /// <summary>
        /// Opens the specified view
        /// </summary>
        /// <param name="view">The view to open and show</param>
        /// <param name="args">Arguments to pass to the view</param>
        void Open(View view, params object[] args);

        void Close(View view);

        bool IsOpened(View view);

        bool IsAnyOpened();

        void CloseAll();
    }
}
