using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSort
{
    public partial class VLC
    {
        public class Preferences
        {
            // string constants as they appear in VLC's config
            internal const string EXTRAINTF = "extraintf";
            internal const string HTTP = "http"; // http server enabled when present on extraintf
            internal const string HTTP_PORT = "http-port";
            internal const string HTTP_PASSWORD = "http-password";

            #region properties

            public string VLCPath { get; set; }
            public string VLCConfigPath { get; set; }
            public bool AutoStart { get; set; }

            // enable: interface > main interfaces > [extra interface modules] > 'web' // todo verify
            // extraintf=http
            public bool HttpEnabled { get; set; } = true; // assumed true until known otherwise
            public string Host { get; set; } = "127.0.0.1";
            public int Port { get; set; } = 9091; // input / codecs > [network settings] > 'http server port'
            public string User { get; set; } = string.Empty; // not currently used, not sure if there is a setting for this
            public string Password { get; set; } = "password"; // interface > main interface > lua > [lua http] > 'password'

            #endregion


            public Preferences()
            {
            }

            /// <summary>
            /// populates a new settings object with values from provided option list
            /// </summary>
            /// <param name="options"></param>
            public Preferences(List<Pref> options)
            {
                foreach (var item in options)
                {
                    if (item.IsOption(HTTP_PORT))
                    {
                        int p;
                        int.TryParse(item.Value, out p);
                        this.Port = p; // 0 if parse failed
                    }
                    else if (item.IsOption(HTTP_PASSWORD))
                    {
                        this.Password = item.Value;
                    }
                    else if (item.IsOption(EXTRAINTF))
                    {
                        var modules = item.Value.Split(':'); // value can have multiple delimited items
                        this.HttpEnabled = modules.Any(n => string.Equals(n, HTTP));
                    }
                }
            }

            internal void LoadConfig()
            {
                this.VLCPath = AppConfig.vlcPath;
                this.VLCConfigPath = AppConfig.vlcrc;
                this.AutoStart = AppConfig.autostart;
                this.Port = AppConfig.port;
                this.User = AppConfig.user;
                this.Password = AppConfig.password;
            }

            /// <summary>
            /// update individual option instances with current settings value
            /// </summary>
            /// <param name="opt">represents a setting/value line in VLC's config</param>
            /// <returns></returns>
            public bool ApplyToSetting(Pref opt)
            {
                bool modified = false;

                if (opt.IsOption(HTTP_PORT))
                {
                    opt.Value = Convert.ToString(this.Port);
                    modified = true;
                }
                else if (opt.IsOption(HTTP_PASSWORD))
                {
                    opt.Value = this.Password?.Trim();
                    modified = true;
                }
                else if (opt.IsOption(EXTRAINTF))
                {
                    // value can have multiple delimited items
                    var modules = new List<string>(opt.Value.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries));

                    // make sure we end up with 0 or 1 occurence of http
                    while (modules.Remove(HTTP)) ;

                    if (this.HttpEnabled)
                    {
                        modules.Add(HTTP); // assuming module order does not matter until I find out otherwise
                    }

                    opt.Value = string.Join(":", modules);
                    modified = true;
                }

                if (modified)
                    opt.IsEnabled = !string.IsNullOrWhiteSpace(opt.Value);

                return modified;
            }

            /// <summary>
            /// verifies settings values are acceptable for VLC http server. does not consider VLC exe/config paths.
            /// </summary>
            /// <param name="msg"></param>
            /// <returns></returns>
            public bool ValidateSettings(out string msg)
            {
                msg = null;

                // inspect values and return t/f for valid settings

                if (this.Port < 1 || this.Port > 65535)
                {
                    msg = $"port # out of range: {this.Port}";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(this.Password))
                {
                    msg = $"password can't be empty";
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(this.User))
                {
                    // user is not supported as there doesn't seem to be a place to set in in VLC
                    // still need an empty value to generate requests
                    msg = $"user should not have a value";
                    return false;
                }

                // host is expected to be 127.0.0.1
                // could be different in vlc prefs // todo

                if (string.IsNullOrWhiteSpace(this.Host))
                {
                    msg = $"host can't be empty";
                    return false;
                }

                return true;
            }

            /// <summary>
            /// generate a list of individual option instances based on current values
            /// </summary>
            /// <returns></returns>
            public List<Pref> ToOptions()
            {
                var list = new List<Pref>();

                foreach (var name in _vlcOptions)
                {
                    var opt = new Pref() { Name = name };
                    this.ApplyToSetting(opt);

                    list.Add(opt);
                }

                return list;
            }
        }

        public class Pref
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public bool IsEnabled { get; set; }

            public Pref()
            {
                // use TryParse() to generate a new instance from a setting line
            }

            /// <summary>
            /// attempt to parse a line of text as a configuration setting
            /// </summary>
            /// <param name="configEntry"></param>
            /// <param name="opt"></param>
            /// <returns></returns>
            public static bool TryParse(string configEntry, out Pref opt)
            {
                opt = null;

                if (string.IsNullOrWhiteSpace(configEntry))
                    return false;

                int i = configEntry.IndexOf('=');
                if (i < 1)
                {
                    return false; // has to have something for the option name preceding = 
                }

                try
                {
                    var tmp = new Pref()
                    {
                        IsEnabled = !configEntry.StartsWith("#"),
                        Name = configEntry.Substring(0, i).TrimStart('#'),
                        // '=' is valid as a config value to have to be careful to preserve
                        Value = configEntry.Substring(i + 1), //.TrimStart('='), 
                    };

                    opt = tmp;
                    return true;
                }
                catch (Exception ex)
                {
                    // todo maybe log or something
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }

            /// <summary>
            /// is this the setting for the provided name
            /// </summary>
            /// <param name="optionName"></param>
            /// <returns></returns>
            internal bool IsOption(string optionName)
            {
                // looks like all option names are entirely lowercase but 
                // this is case insensitve assuming there won't be 2 with the same name
                return string.Equals(this.Name, optionName, StringComparison.InvariantCultureIgnoreCase);
            }

            public override string ToString()
            {
                // format as exists in vlc config file
                string hash = this.IsEnabled ? "" : "#";
                string s = $"{hash}{this.Name}={this.Value}";
                return s;
            }
        }
    }
}
