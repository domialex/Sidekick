using System.Collections.Generic;

namespace Sidekick.Domain.Cheatsheets.Betrayal
{
    public class BetrayalLeague
    {
        public List<BetrayalAgent> Agents { get; private set; } = new List<BetrayalAgent>();
    }
}
