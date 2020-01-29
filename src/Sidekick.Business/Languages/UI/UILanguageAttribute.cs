using System;
using System.Globalization;

namespace Sidekick.Business.Languages.UI
{
    public class UILanguageAttribute : Attribute
    {
        public UILanguageAttribute(string name)
        {
            var culture = new CultureInfo(name);
            DisplayName = culture.NativeName;
            Name = culture.Name;
        }

        public string DisplayName { get; private set; }
        public string Name { get; private set; }
        public Type ImplementationType { get; set; }
    }
}
