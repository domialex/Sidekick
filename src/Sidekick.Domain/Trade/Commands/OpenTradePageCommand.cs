using MediatR;

namespace Sidekick.Domain.Trade.Commands
{
    /// <summary>
    /// Opens the official trade page for the item under the cursor inside Path of Exile
    /// </summary>
    public class OpenTradePageCommand : ICommand<bool>
    {
    }
}
