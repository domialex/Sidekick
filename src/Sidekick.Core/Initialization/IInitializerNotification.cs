using System;
using MediatR;

namespace Sidekick.Core.Initialization
{
    public interface IInitializerNotification : INotification
    {
        Action<string> OnStart { get; set; }
        Action<string> OnEnd { get; set; }
    }
}
