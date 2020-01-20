namespace Sidekick.Core.Settings
{
    public static class SettingExtensions
    {
        public static string GetTemplate(this GeneralSetting setting)
        {
            return "{{GeneralSetting:" + setting.ToString() + "}}";
        }

        public static string GetTemplate(this KeybindSetting setting)
        {
            return "{{KeybindSetting:" + setting.ToString() + "}}";
        }
    }
}
