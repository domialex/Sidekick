using System.Collections.Generic;
using MediatR;

namespace Sidekick.Domain.Cheatsheets.Betrayal
{
    /// <summary>
    /// Gets the betrayal sort options
    /// </summary>
    public class GetBetrayalSortOptionsQuery : IQuery<Dictionary<string, string>>
    {
    }
}
