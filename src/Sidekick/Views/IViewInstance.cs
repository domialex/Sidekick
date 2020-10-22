using System;

namespace Sidekick.Views
{
    public interface IViewInstance : IDisposable
    {
        Type Type { get; }
    }
}
