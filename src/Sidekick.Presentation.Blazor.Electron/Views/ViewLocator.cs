using System;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        public ViewLocator()
        {
        }

        public void Open(View view, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool IsOpened(View view) => throw new NotImplementedException();

        public bool IsAnyOpened() => throw new NotImplementedException();

        public void CloseAll()
        {
            throw new NotImplementedException();
        }

        public void Close(View view)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CloseAll();
        }
    }
}
