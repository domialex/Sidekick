using System.Collections.Generic;

namespace Sidekick.Modules.Cheatsheets.Delve
{
    public class RegionModel
    {
        public RegionModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<FossilModel> Fossils { get; set; } = new List<FossilModel>();
    }
}
