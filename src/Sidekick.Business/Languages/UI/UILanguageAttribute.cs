using System;

namespace Sidekick.Business.Languages.UI
{
    public class UILanguageAttribute : Attribute
    {
        public UILanguageAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public Type ImplementationType { get; set; }
    }
}
