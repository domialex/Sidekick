namespace Sidekick.Business.Apis.Poe.Parser
{
    public readonly struct ItemSections
    {

        /// <summary>
        /// Sections containing arrays of individual lines. For accessing specific lines.
        /// </summary>
        public string[][] SplitSections { get; }

        /// <summary>
        /// Sections with the contents as single string. For parsing the whole section.
        /// </summary>
        public string[] WholeSections { get; }

        public ItemSections(string[][] splitSections, string[] wholeSections)
        {
            SplitSections = splitSections;
            WholeSections = wholeSections;
        }

        public string[] HeaderSection => SplitSections[0];

        public string Rarity => HeaderSection[0];

        public string MapPropertiesSection => WholeSections[1];

        public string MapInfluenceSection => WholeSections[3];

        public string NameLine => HeaderSection.Length == 3 ? HeaderSection[1] : string.Empty;

        public string TypeLine => HeaderSection.Length == 3 ? HeaderSection[2] : string.Empty;

        public bool TryGetVaalGemName(out string gemName)
        {
            if(SplitSections.Length > 7)
            {
                gemName = SplitSections[5][0];
                return true;
            }

            gemName = null;
            return false;
        }

        public bool TryGetMapTierLine(out string mapTierLine)
        {
            if(SplitSections.Length > 1)
            {
                mapTierLine = SplitSections[1][0];
                return true;
            }

            mapTierLine = null;
            return false;
        }
    }
}
