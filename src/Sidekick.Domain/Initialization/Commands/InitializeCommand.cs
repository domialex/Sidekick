using MediatR;

namespace Sidekick.Domain.Initialization.Commands
{
    public class InitializeCommand : ICommand
    {
        public InitializeCommand(bool firstRun)
        {
            FirstRun = firstRun;
        }

        public bool FirstRun { get; }
    }
}
