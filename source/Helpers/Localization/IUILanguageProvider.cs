using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.Localization
{
    public interface IUILanguageProvider
    {
        string ShowLog { get; }
        string Exit { get; }
        string League { get; }
        string LabelAccountName { get; }
        string LabelCharacter { get; }
        string LabelPrice { get; }
        string LabelItemLevel { get; }
        string LabelAge { get; }
        string StartTooltip { get; }
        string StartTooltipCaption { get; }
    }
}
