using System.Collections.Generic;

namespace Sidekick.Business.Maps
{
    public interface IMapService
    {
        HashSet<string> MapNames { get; }
    }
}
