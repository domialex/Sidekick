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
        /// <param name="autoUpdate">Indicates if we should auto update</param>
        public InitializeCommand(bool firstRun, bool autoUpdate)
        {
            FirstRun = firstRun;
            AutoUpdate = autoUpdate;
        }

        /// <summary>
        /// Indicates if this command is called at the start of the application, or after some setting changes
        /// </summary>
        public bool FirstRun { get; }

        /// <summary>
        /// Indicates if we should auto update
        /// </summary>
        public bool AutoUpdate { get; }
    }
}
