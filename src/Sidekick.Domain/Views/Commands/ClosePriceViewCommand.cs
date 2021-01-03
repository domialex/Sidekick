using MediatR;

namespace Sidekick.Domain.Views.Commands
{
    /// <summary>
    /// Close the price view
    /// </summary>
    public class ClosePriceViewCommand : ICommand<bool>
    {
    }
}
