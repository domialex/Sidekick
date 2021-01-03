using MediatR;

namespace Sidekick.Domain.Views.Commands
{
    /// <summary>
    /// Close the map view
    /// </summary>
    public class CloseMapViewCommand : ICommand<bool>
    {
    }
}
