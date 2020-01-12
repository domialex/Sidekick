using Newtonsoft.Json;
using Sidekick.Helpers;
using System;
using System.IO;
using System.Windows.Input;

namespace Sidekick.Windows.Settings
{
    public static class SettingsController
    {
        private static SettingsView _settingsView;
        public static bool IsDisplayed => _settingsView != null && _settingsView.IsDisplayed;
        private static int WINDOW_WIDTH = 480;
        private static int WINDOW_HEIGHT = 320;
        public static void Show()
        {
            if (_settingsView == null)
            {
                _settingsView = new SettingsView(WINDOW_WIDTH, WINDOW_HEIGHT);
            }

            _settingsView.Activate();
            _settingsView.OnWindowClosed += (s, e) => _settingsView = null;
        }

        private static readonly string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        private static Models.Settings settings;

        /// <summary>
        /// Loads the default settings
        /// </summary>
        /// <returns></returns>
        private static Models.Settings LoadDefaultSettings()
        {
            try
            {
                Logger.Log("Loading default settings");
                var settings = new Models.Settings();
                settings.KeybindSettings.Add(Models.KeybindSetting.CloseWindow, new Models.Hotkey(System.Windows.Forms.Keys.Escape, System.Windows.Forms.Keys.None));
                settings.KeybindSettings.Add(Models.KeybindSetting.PriceCheck, new Models.Hotkey(System.Windows.Forms.Keys.D, System.Windows.Forms.Keys.Control));
                settings.KeybindSettings.Add(Models.KeybindSetting.Hideout, new Models.Hotkey(System.Windows.Forms.Keys.F5, System.Windows.Forms.Keys.None));
                settings.KeybindSettings.Add(Models.KeybindSetting.ItemWiki, new Models.Hotkey(System.Windows.Forms.Keys.W, System.Windows.Forms.Keys.Alt));
                // TODO: Add more default settings
                return settings;
            }
            catch (Exception)
            {
                Logger.Log("Could not load default settings", LogState.Error);
                throw;
            }
        }

        public static Models.Settings GetSettingsInstance()
        {
            if(settings == null)
            {
                return LoadSettings();
            }
            else
            {
                return settings;
            }
        }

        public static void CaptureKeyEvents(System.Windows.Forms.Keys key, System.Windows.Forms.Keys modifier)
        {
            _settingsView.CaptureKeyEvents(key, modifier);
        }

        /// <summary>
        /// Loads the settings from the settings file. If no settings file exists, loads default settings
        /// </summary>
        /// <returns></returns>
        public static Models.Settings LoadSettings()
        {
            try
            {
                Logger.Log("Loading settings");
                string settingsString = null;
                if (File.Exists(settingsPath))
                {
                    settingsString = File.ReadAllText(settingsPath);
                }

                //If settings have never been initialized, create new settings, otherwise clear and reload settings
                if (settings == null) settings = new Models.Settings();
                else settings.Clear();

                if (String.IsNullOrEmpty(settingsString))
                {
                    settings = LoadDefaultSettings();
                }
                else
                {
                    settings = JsonConvert.DeserializeObject<Models.Settings>(settingsString);
                    // TODO: Add new settings, that aren't in the settings file yet
                }
                return settings;
            }
            catch (Exception)
            {
                Logger.Log("Could not load settings", LogState.Error);
                throw;
            }
        }

        /// <summary>
        /// Backups the current settings file and saves settings
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                Logger.Log("Saving settings");
                // Backup old settings
                if (File.Exists(settingsPath))
                {
                    File.Copy(settingsPath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json.old"), true);
                    File.Delete(settingsPath);
                }

                string settingsString = JsonConvert.SerializeObject(settings);
                File.WriteAllText(settingsPath, settingsString);
            }
            catch (Exception)
            {
                Logger.Log("Could not save settings", LogState.Error);
                throw;
            }
        }
    }
}
