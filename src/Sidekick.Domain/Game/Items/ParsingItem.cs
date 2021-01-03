namespace Sidekick.Domain.Game.Items
{
    public readonly struct ParsingItem
    {
        public ParsingItem(string[] wholeSections, string[][] splitSections, string text)
        {
            WholeSections = wholeSections;
            SplitSections = splitSections;
            Text = text;
        }

        /// <summary>
        /// Sections containing arrays of individual lines. For accessing specific lines.
        /// </summary>
        public string[][] SplitSections { get; }

        /// <summary>
        /// Sections with the contents as single string. For parsing the whole section.
        /// </summary>
        public string[] WholeSections { get; }

        public string Text { get; }

        public string[] HeaderSection => SplitSections[0];

        public string Rarity => HeaderSection[0];

        public string MapPropertiesSection => WholeSections[1];

        public string NameLine => HeaderSection.Length > 1 ? HeaderSection[1] : string.Empty;

        public string TypeLine => HeaderSection.Length > 2 ? HeaderSection[2] : string.Empty;
    }
}
