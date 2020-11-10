using MediatR;

namespace Sidekick.Domain.Wikis.Commands
{
    /// <summary>
    /// Opens a webpage to the wiki of the item under the cursor inside Path of Exile
    /// </summary>
    public class OpenWikiPageCommand : ICommand<bool>
    {
    }
}
