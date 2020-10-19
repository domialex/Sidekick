using System;
using System.Text.Json.Serialization;

namespace Sidekick.Core.Initialization
{
    /// <summary>
    /// Tries to update the application during the application initialization
    /// </summary>
    public class InitializeUpdateNotification : IInitializerNotification
    {
        [JsonIgnore]
        public Action<string> OnStart { get; set; }

        [JsonIgnore]
        public Action<string> OnEnd { get; set; }
    }
}
