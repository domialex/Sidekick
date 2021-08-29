using System.Collections.Generic;
using System.Reflection;

namespace Sidekick.Common.Blazor
{
    public class SidekickModule
    {
        public static List<SidekickModule> Modules { get; } = new();

        public Assembly Assembly { get; init; }
    }
}
