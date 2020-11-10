using System;

namespace Sidekick.Domain.Game.Languages
{
    public class LanguageAttribute : Attribute
    {
        public LanguageAttribute(string name, string descriptionRarity, string languageCode)
        {
            Name = name;
            DescriptionRarity = descriptionRarity;
            LanguageCode = languageCode;
        }

        public string Name { get; private set; }
        public string DescriptionRarity { get; private set; }
        public string LanguageCode { get; private set; }
        public Type ImplementationType { get; set; }
    }
}
