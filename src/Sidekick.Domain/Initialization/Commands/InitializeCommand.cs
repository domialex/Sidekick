using MediatR;

namespace Sidekick.Domain.Initialization.Commands
{
    /// <summary>
    /// Command to initialize the application
    /// </summary>
    public class InitializeCommand : ICommand
    {
        /// <summary>
        /// Command to initialize the application
        /// </summary>
        /// <param name="firstRun">Indicates if this command is called at the start of the application, or after some setting changes</param>
        public InitializeCommand(bool firstRun)
        {
            FirstRun = firstRun;
        }

        /// <summary>
        /// Indicates if this command is called at the start of the application, or after some setting changes
        /// </summary>
        public bool FirstRun { get; }
    }
}
