using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AudioSort
{
    public static class AppConfig
    {
        // key names as they appear in app.config
        public const string HOST = "host";
        public const string PORT = "port";
        public const string USER = "user";
        public const string PASSWORD = "password";
        public const string VLC_PATH = "vlcPath";
        public const string VLC_CONFIG_PATH = "vlcrc";
        public const string AUTOSTART = "autostart";
        public const string CONFIRM_SOUND = "confirmSound";
        public const string PROBLEM_SOUND = "problemSound";

        private static Configuration _activeConfig;

        public static Configuration ActiveConfig
        {
            get
            {
                if (_activeConfig == null)
                    _activeConfig = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);

                if (_activeConfig.AppSettings == null || _activeConfig.AppSettings.Settings == null)
                    throw new Exception("appSettings null no idea");

                return _activeConfig;
            }
            set { _activeConfig = value; }
        }

        // values read from app.config or internal default if not present

        public static string host
        {
            get
            {
                var host = strNotNull(HOST);
                if (!string.IsNullOrWhiteSpace(host))
                {
                    return host;
                }
                return "127.0.0.1";
            }
        }

        public static int port
        {
            get
            {
                int port;
                if (int.TryParse(str(PORT), out port))
                {
                    return port;
                }
                return 9091; // internal default
            }
        }

        public static string user { get { return strNotNull(USER); } }

        public static string password
        {
            get
            {
                var pass = strNotNull(PASSWORD);
                if (!string.IsNullOrWhiteSpace(pass))
                {
                    return pass;
                }
                return "password";
            }
        }

        // vlc.exe path
        public static string vlcPath { get { return str(VLC_PATH); } }

        // default vlcrc path is managed elsewhere, this is only for the app.config value
        public static string vlcrc { get { return str(VLC_CONFIG_PATH); } }

        public static bool autostart
        {
            get
            {
                bool b;
                if (bool.TryParse(str(AUTOSTART), out b))
                {
                    return b;
                }
                return false;
            }
        }

        public static string confirmSound { get { return strNotNull(CONFIRM_SOUND); } }

        public static string problemSound { get { return strNotNull(PROBLEM_SOUND); } }

        public static string GetCrateFolder(int i)
        {
            var path = str($"crateFolder{i}");

            if (string.IsNullOrWhiteSpace(path))
                path = str($"txtFolder{i}");

            return path?.Trim();
        }


        /// <summary>
        /// update the app.config. empty strings are valid values. a null value will remove a key. 
        /// </summary>
        /// <param name="values"></param>
        internal static void UpdateConfigFile(Dictionary<string, string> values)
        {
            var config = ActiveConfig;

            foreach (var item in values)
            {
                var key = item.Key;
                var val = item.Value?.Trim();

                config.AppSettings.Settings.Remove(key);

                if (val != null) // null used to indicate a key should not be re-added
                    config.AppSettings.Settings.Add(key, val);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            // necessary to reopen?
            //ActiveConfig = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
        }


        /// <summary>
        /// shorthand method for getting a config value. null if key not present.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string str(string key)
        {
            string s = null;

            var kv = ActiveConfig.AppSettings.Settings[key];
            if (kv != null)
                s = kv.Value?.Trim();

            return s;
        }

        /// <summary>
        /// shorthand method for getting a config value. defaults to empty string if key not present.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string strNotNull(string key)
        {
            var s = str(key);
            if (s == null)
                s = string.Empty;

            return s;
        }
    }
}
