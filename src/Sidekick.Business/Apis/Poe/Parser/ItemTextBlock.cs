using System;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public readonly struct ItemTextBlock
    {
        public string[] Header { get; }

        public string[][] Blocks { get; }

        public bool IsVaalGem(Rarity rarity) => rarity == Rarity.Gem && Blocks.Length > 6;

        public string VaalGemName => Blocks.Length > 6 ? Blocks[4][0] : throw new Exception("Tried getting VaalGemName from non-Vaal Gem item");

        public ItemTextBlock(string[][] text)
        {
            Header = text[0];
            Blocks = text[1..];
        }
    }
}
