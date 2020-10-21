using MediatR;

namespace Sidekick.Domain.Initialization.Commands
{
    public class InitializeCommand : IRequest
    {
        public InitializeCommand(bool firstRun)
        {
            FirstRun = firstRun;
        }

        public bool FirstRun { get; }
    }
}
