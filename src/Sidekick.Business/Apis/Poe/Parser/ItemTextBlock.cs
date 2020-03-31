namespace Sidekick.Business.Apis.Poe.Parser
{
    public readonly struct ItemTextBlock
    {

        /// <summary>
        /// Blocks containing arrays of individual lines. For accessing specific lines.
        /// </summary>
        public string[][] SplitBlocks { get; }

        /// <summary>
        /// Blocks with the contents as single string. For parsing the whole block.
        /// </summary>
        public string[] WholeBlocks { get; }

        public ItemTextBlock(string[][] splitBlocks, string[] wholeBlocks)
        {
            SplitBlocks = splitBlocks;
            WholeBlocks = wholeBlocks;
        }

        public string[] Header => SplitBlocks[0];

        public string Rarity => Header[0];

        public string MapPropertiesBlock => WholeBlocks[1];

        public bool TryGetVaalGemName(out string gemName)
        {
            if(SplitBlocks.Length > 7)
            {
                gemName = SplitBlocks[5][0];
                return true;
            }

            gemName = null;
            return false;
        }

        public bool TryGetMapTierLine(out string mapTierLine)
        {
            if(SplitBlocks.Length > 1)
            {
                mapTierLine = SplitBlocks[1][0];
                return true;
            }

            mapTierLine = null;
            return false;
        }

        
    }
}
