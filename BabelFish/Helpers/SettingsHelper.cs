using System.Reflection;
using System.Configuration;
using BabelFish.DataModel;
using BabelFish.DataModel.Definitions;
using System.Collections.Specialized;

namespace BabelFish.Helpers
{
    static class SettingsHelper
    {
        #region Properties
        /// <summary>
        /// Define what settings the user can pass in
        /// typeof for validation
        /// Any extraneous settings will be ignored
        /// Set options: 1) App.config 2) BabelFish_User_Settings.config 3) IncomingUserSettings array
        /// </summary>
        private static Dictionary<string, Type> AllowedSettings = new Dictionary<string, Type>()
        {
            { "UserName", typeof(string) },                 // User Name to validate AWS access
            { "PassWord", typeof(string) },                 // User Password to validate AWS access
            { "AccessKey", typeof(string) },                // User AccessKey for valid AWS access
            { "SecretKey", typeof(string) },                // User SecretKey for valid AWS access
            { "Logging_NLogConfig", typeof(string) },       // Alternate NLog path/filename
            { "Definitions_CacheAlwaysNew", typeof(bool) }, // Definitions cache override to always fetch new
            { "XApiKey", typeof(string) },                  // User XApiKey passed in
        };

        /// <summary>
        /// Holds user settings for use in application
        /// dynamic value is null if key not found in inputs
        /// </summary>
        public static Dictionary<string, dynamic> UserSettings = new Dictionary<string, dynamic>();

        /// <summary>
        /// User Settings passed in as App.config alternative
        /// </summary>
        private static Dictionary<string, string> _IncomingUserSettings = new Dictionary<string, string>();
        public static Dictionary<string, string> IncomingUserSettings
        {
            get
            {
                return _IncomingUserSettings;
            }
            set
            {
                _IncomingUserSettings = value;
                LoadApplicationSettings();
            }
        }
        #endregion Properties
        
        static SettingsHelper() {
            LoadApplicationSettings();
        }

        #region Methods
        private static void LoadApplicationSettings()
        {
            try
            {
                ResetUserSettings();

                foreach (KeyValuePair<string, Type> kvp in AllowedSettings)
                {
                    // Primary check is app.config and User Settings file if they exist
                    if ( ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings.Count > 0 )
                        UserSettings[kvp.Key] = ConvertSettingsType(kvp.Value, ConfigurationManager.AppSettings[kvp.Key]);
                    // Secondary is incoming User Settings Dictionary if it exists
                    if (IncomingUserSettings.ContainsKey(kvp.Key))
                        UserSettings[kvp.Key] = ConvertSettingsType(kvp.Value, IncomingUserSettings[kvp.Key]);
                }

            } finally { }
        }

        private static void ResetUserSettings()
        {
            try
            {
                UserSettings.Clear();
                foreach (KeyValuePair<string, Type> kvp in AllowedSettings)
                    UserSettings.Add(kvp.Key, null);

            } finally { }
        }

        /// <summary>
        /// Convert setting Value to specific Type
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="setting"></param>
        /// <returns>Type value or null if cannot find/convert</returns>
        private static dynamic ConvertSettingsType(Type newType, string setting)
        {
            dynamic r = null;
            try
            {
                if (newType == typeof(string))
                    r = setting;
                else if (newType == typeof(bool)) {
                    if (Boolean.TryParse(setting, out bool tryBool))
                        r = tryBool;
                } else if (newType == typeof(int)) {
                    if (int.TryParse(setting, out int tryInt))
                        r = tryInt;
                } else if (newType == typeof(float)) {
                    if (float.TryParse(setting, out float tryFloat))
                        r = tryFloat;
                }
            } finally { }

            return r;
        }

        public static bool SettingIsNullOrEmpty(string settingName)
        {
            if ((UserSettings.ContainsKey(settingName)) && UserSettings[settingName] == null || UserSettings[settingName] == string.Empty)
                return true;
            else
                return false;
        }
        #endregion Methods

        public static Dictionary<string, string> RevertSettingsFormat()
        {
            Dictionary<string, string> revertSettings = new Dictionary<string, string>();
            foreach (KeyValuePair<string, dynamic> loopit in Helpers.SettingsHelper.UserSettings)
                revertSettings[loopit.Key] = loopit.Value;

            return revertSettings;
        }
    }
}
