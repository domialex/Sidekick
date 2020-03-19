using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Windows.AdvancedSearch.ViewModels
{
    public class SearchAttribute
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool IsEnabled { get; set; }
    }
}
