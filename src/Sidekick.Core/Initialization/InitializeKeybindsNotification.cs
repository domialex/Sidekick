using System;
using System.Text.Json.Serialization;

namespace Sidekick.Core.Initialization
{
    /// <summary>
    /// Binds keybinds during the application initialization
    /// </summary>
    public class InitializeKeybindsNotification : IInitializerNotification
    {
        [JsonIgnore]
        public Action<string> OnStart { get; set; }

        [JsonIgnore]
        public Action<string> OnEnd { get; set; }
    }
}