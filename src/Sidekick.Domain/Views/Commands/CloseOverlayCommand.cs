using MediatR;

namespace Sidekick.Domain.Views.Commands
{
    /// <summary>
    /// Close the map view
    /// </summary>
    public class CloseOverlayCommand : ICommand<bool>
    {
    }
}
