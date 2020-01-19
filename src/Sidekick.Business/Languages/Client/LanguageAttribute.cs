using System;

namespace Sidekick.Business.Languages.Client
{
    public class LanguageAttribute : Attribute
    {
        public LanguageAttribute(string name, string rarityDescription)
        {
            Name = name;
            DescriptionRarity = rarityDescription;
        }

        public string Name { get; private set; }
        public string DescriptionRarity { get; private set; }
        public Type ImplementationType { get; set; }
    }
}
