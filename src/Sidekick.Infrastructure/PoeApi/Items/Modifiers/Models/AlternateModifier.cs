using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Sidekick.Domain.Game.StatTranslations;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class AlternateModifier
    {
        public Stat Stat { get; set; }

        public Regex Pattern { get; set; }
    }
}
