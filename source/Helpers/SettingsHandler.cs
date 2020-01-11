using Newtonsoft.Json;
using Sidekick.Helpers.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers
{
    public class SettingsHandler
    {
        public const string SettingsFileName = "settings.json";

        public string SettingsPath { get; private set; }
        public Settings CurrentSettings { get; private set; }

        public SettingsHandler()
        {
            SettingsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SettingsFileName);
        }

        public void ReadSettings()
        {
            if(!File.Exists(SettingsPath))
            {
                // Init Settings with default settings
                CurrentSettings = new Settings()
                {
                    UILanguage = "German",
                };
            }
            else
            {
                var fileContent = File.ReadAllText(SettingsPath);
                CurrentSettings = JsonConvert.DeserializeObject<Settings>(fileContent);
            }
        }

        public void WriteSettings()
        {
            throw new NotImplementedException();
        }

        public void HandleCurrentSettings()
        {
            if(CurrentSettings != null)
            {
                // UI language handling
                if(!Enum.TryParse(CurrentSettings.UILanguage, out Language lang))
                {
                    Logger.Log("Couldn't parse UI language settings, defaulting to english", LogState.Error);
                    lang = Language.English;
                }

                LanguageSettings.ChangeUILanguage(lang);
            }
        }

        private void ChangeSettingsValue(string settingName, object value)
        {

        }
    }

    public class Settings
    {
        [JsonProperty(PropertyName = "ui_language")]
        public string UILanguage { get; set; }
    }
}
