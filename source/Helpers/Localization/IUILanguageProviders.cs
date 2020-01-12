using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.Localization
{
    public class UILanguageProvidersDE : IUILanguageProvider
    {
        public string ShowLog => "Log anzeigen";
        public string Exit => "Beenden";
        public string League => "League";       // Liga ?
        public string LabelAccountName => "Account Name";
        public string LabelCharacter => "Charakter";
        public string LabelPrice => "Preis";
        public string LabelItemLevel => "iLvl";
        public string LabelAge => "Alter";
        public string StartTooltip => "Drücke Strg+D über einem Item während des Spielens. Drücke Esc um das Overlay zu schließen";
        public string StartTooltipCaption => "Sidekick ist bereit";
    }

    public class UILanguageProviderEN : IUILanguageProvider
    {
        public string ShowLog => "Show Log";
        public string Exit => "Exit";
        public string League => "League";
        public string LabelAccountName => "Account Name";
        public string LabelCharacter => "Character";
        public string LabelPrice => "Price";
        public string LabelItemLevel => "iLvl";
        public string LabelAge => "Age";
        public string StartTooltip => "Press Ctrl+D over an item in-game to use. Press Escape to close overlay.";
        public string StartTooltipCaption => "Sidekick is ready";
    }
}
