using System;
using MediatR;
using Sidekick.Core.Initialization.Notifications;

namespace Sidekick.Core.Initialization
{
    public class InitializeCommand : IRequest
    {
        public InitializeCommand(bool firstRun)
        {
            FirstRun = firstRun;
        }

        public bool FirstRun { get; }
        public Action OnError { get; set; }
        public Action<ProgressNotification> OnProgress { get; set; }
    }
}
