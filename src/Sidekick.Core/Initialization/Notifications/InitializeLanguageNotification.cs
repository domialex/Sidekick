using System;
using System.Text.Json.Serialization;

namespace Sidekick.Core.Initialization.Notifications
{
    /// <summary>
    /// Initializes languages during the application initialization
    /// </summary>
    public class InitializeLanguageNotification : IInitializerNotification
    {
        [JsonIgnore]
        public Action<string> OnStart { get; set; }

        [JsonIgnore]
        public Action<string> OnEnd { get; set; }
    }
}
