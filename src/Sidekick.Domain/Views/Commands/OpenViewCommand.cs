using MediatR;

namespace Sidekick.Domain.Views.Commands
{
    /// <summary>
    /// Opens the specified view
    /// </summary>
    public class OpenViewCommand : ICommand<bool>
    {
        /// <summary>
        /// Opens the specified view
        /// </summary>
        /// <param name="view">The view to open and show</param>
        public OpenViewCommand(View view)
        {
            View = view;
        }

        /// <summary>
        /// The view to open and show
        /// </summary>
        public View View { get; }
    }
}
