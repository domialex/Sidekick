using System;
using System.Text.Json.Serialization;

namespace Sidekick.Core.Initialization.Notifications
{
    /// <summary>
    /// Initializes settings during the application initialization
    /// </summary>
    public class InitializeSettingsNotification : IInitializerNotification
    {
        [JsonIgnore]
        public Action<string> OnStart { get; set; }

        [JsonIgnore]
        public Action<string> OnEnd { get; set; }
    }
}
