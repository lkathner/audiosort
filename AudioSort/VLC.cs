using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioSort
{
    public partial class VLC
    {

        /* todo remove after commit
        private static string _host = "127.0.0.1"; // todo
        private static string _port = "9091"; // input / codecs > [network settings] > 'http server port'
        private static string _user = ""; // ?
        private static string _password = "password"; // interface > main interface > lua > [lua http] > 'password'
        */

        private static readonly string[] _vlcOptions; // http-related vlc option names
        private static Preferences _settings; // vlc connection values
        private static bool _rebootVLC = false;

        public static Preferences Settings
        {
            get { return _settings; }
        }

        static VLC()
        {
            _vlcOptions = new string[]
            {
                Preferences.HTTP_PORT,
                Preferences.HTTP_PASSWORD,
                Preferences.EXTRAINTF,
            };

            _settings = new Preferences();
        }

        private static void Proc_Exited(object sender, EventArgs e)
        {
            var reboot = _rebootVLC;
            _rebootVLC = false;

            var proc = GetProcess();
            if (proc != null)
                return; // an instance of vlc is already running, don't try to start it again

            if (reboot)
                Launch(); // restart
        }


        /* todo remove after commit
        public static void Set(string port, string user, string pass)
        {
            _port = port?.Trim() ?? string.Empty;
            _user = user?.Trim() ?? string.Empty;
            _password = pass?.Trim() ?? string.Empty;
        }
        */

        private static async Task<string> GetPlaylistJson2()
        {
            string json = await VLCRequest("playlist.json");
            return json;
        }

        private static async Task<string> GetStatusJson()
        {
            string json = await VLCRequest("status.json");
            return json;
        }

        private static async Task<string> VLCRequest(string item)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    string encoded = GetUserPasswordEncoded();
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                    var response = await client.GetAsync($@"http://{_settings.Host}:{_settings.Port}/requests/{item}");
                    string json = await response.Content.ReadAsStringAsync();

                    // if we get something back but it's not json, it's probably a bad credentials response
                    if (json.Contains("401 Client error"))
                        throw new Exception($"invalid password"); // assuming

                    return json;
                }
            }
            catch (Exception ex)
            {
                var x = ex;
                while (x != null)
                {
                    if ((uint)x.HResult == 0x80004005)    // connection refused
                        ThrowConnectionRefused(ex);
                    x = x.InnerException;
                }
                throw; // rethrow exception as caught
            }
        }

        private static string GetUserPasswordEncoded()
        {
            //string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            //var b = Convert.FromBase64String("OnBhc3N3b3Jk");
            //var s = Encoding.GetEncoding("ISO-8859-1").GetString(b);

            var creds = $"{_settings.User}:{_settings.Password}";

            var enc = Encoding.GetEncoding("ISO-8859-1"); //.GetString(b);
            var b = enc.GetBytes(creds);

            string encoded = System.Convert.ToBase64String(b);
            return encoded;
        }

        private static void ThrowConnectionRefused(Exception ex)
        {
            var proc = GetProcess();
            if (proc == null)
            {
                // not running as far as we can tell
                throw new Exception("VLC not running", ex);
            }

            // vlc running but connection unavailable

            string msg;
            var setup = ReadVLCConfigFile(out msg);
            if (setup != null)
            {
                if (!setup.HttpEnabled) // vlc not configured for http 
                    throw new Exception("VLC http disabled", ex);

                if (AppConfig.port != setup.Port) // port value set wrong
                    throw new Exception($"port mismatch, VLC={setup.Port} AudioSort={AppConfig.port}", ex);

                // TODO other reason?
            }
            else
            {
                // config file not available (only non-thrown reason to not get settings back)
                // can't compare setup for specific issues
            }

            // can't auto verify anything  
            throw new Exception("couldn't connect, check setup", ex);
        }

        /* todo remove
        private static Exception WrapException(Exception ex)
        {
            var x = ex;
            while (x != null)
            {
                if ((uint)x.HResult == 0x80004005) 	  // connection refused
                {
                    var msg = "VLC not running";
                    throw new Exception(msg, ex);
                }

                x = x.InnerException;
            }

            return ex;
        }
        */


        public static async Task<Playlist> GetPlaylistInfo()
        {
            var json = await GetPlaylistJson2();
            var pl3 = new Playlist(json);
            return pl3;
        }

        /* this is to keep the diff squared up for the commit todo remove after commmit
        void zzz()
        {
            try
            {
            }
            catch (Exception ex)
            {
                var ex2 = (ex);
                if (ex2 != ex)
                {
                    throw ex2;
                }

                throw;
            }
        }
        */

        public static async Task<Status> GetCurrentStatus()
        {
            var json = await VLC.GetStatusJson();
            var status = new Status(json);
            return status;
        }

        /* this is to keep the diff squared up for the commit todo remove after commmit
        void zzz2()
        {
            try
            {
            }
            catch (Exception ex)
            {
                var ex2 = (ex);
                if (ex2 != ex)
                {
                    throw ex2;
                }

                throw;
            }
        }
        */

        /// <summary>
        /// prepare app for vlc connections
        /// </summary>
        /// <returns></returns>
        internal static bool LoadConfig()
        {
            _settings = new Preferences();
            _settings.LoadConfig();

            string msg;
            return _settings.ValidateSettings(out msg);
        }

        /// <summary>
        /// start vlc.exe or verify it's already running
        /// </summary>
        /// <returns></returns>
        internal static bool Launch()
        {
            var proc = GetProcess();
            if (proc == null) // is not already running
            {
                if (string.IsNullOrWhiteSpace(_settings.VLCPath))
                    throw new Exception("VLC path not set");

                if (!File.Exists(_settings.VLCPath))
                    throw new Exception("VLC path invalid");

                proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = _settings.VLCPath;
                proc.Start();
            }

            if (proc == null) // todo or status ?
                return false;

            return true;
        }

        /// <summary>
        /// end vlc if it's running (assumes only one instance at a time)
        /// </summary>
        /// <param name="reboot"></param>
        internal static void Close(bool reboot = true)
        {
            var proc = GetProcess();
            if (proc == null) // not running
            {
                _rebootVLC = false;
                if (reboot)
                    Launch();

                return;
            }

            if (_rebootVLC = reboot)
                proc.Exited += Proc_Exited; // reboot after running instances exists

            // ask nice
            bool exited = false;
            var sent = proc.CloseMainWindow();
            if (sent)
            {
                exited = proc.WaitForExit(2000);
            }

            if (!sent || !exited)
            {
                //proc.Kill(); // force it
            }

            if (proc != null && !proc.HasExited)
            {
                _rebootVLC = false; // something is preventing close, let user deal with it
            }
        }

        /// <summary>
        /// check for running vlc process
        /// </summary>
        /// <returns></returns>
        internal static System.Diagnostics.Process GetProcess()
        {
            var procs = System.Diagnostics.Process.GetProcessesByName("vlc");
            if (procs.Length > 0)
            {
                return procs[0];
            }
            return null;
        }

        /// <summary>
        /// get vlc.exe path from running process
        /// </summary>
        /// <returns></returns>
        internal static string GetProcessPath()
        {
            var vlcProc = VLC.GetProcess();
            if (vlcProc != null)
            {
                return vlcProc.MainModule.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// get default path to vlc's config file 'vlcrc'
        /// </summary>
        /// <returns></returns>
        internal static string GetVLCSettingsPath()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string file = Path.Combine(appdata, "vlc", "vlcrc");
            return file;
        }

        /// <summary>
        /// read configured values from vlcrc (the ones useful to this app)
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal static Preferences ReadVLCConfigFile(out string msg)
        {
            var file = _settings.VLCConfigPath;
            if (string.IsNullOrWhiteSpace(file))
                file = VLC.GetVLCSettingsPath();

            msg = null;
            if (!File.Exists(file))
            {
                msg = $"VLC config file not at expected location {file}";
                return null;
            }

            var list = new List<Pref>();
            using (var reader = new StreamReader(file))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    Pref opt;
                    if (Pref.TryParse(line, out opt) && _vlcOptions.Any(n => opt.IsOption(n)))
                    {
                        list.Add(opt); // gathering only relevant settings
                    }
                }
            }

            var settings = new Preferences(list);
            return settings;
        }

        /// <summary>
        /// modify vlcrc with given values
        /// </summary>
        /// <param name="settings"></param>
        internal static void WriteVLCConfigFile(Preferences settings)
        {
            // todo overload that takes list of individual options, for flexibility

            //string file = GetVLCSettingsPath();
            var file = _settings.VLCConfigPath;
            if (string.IsNullOrWhiteSpace(file))
                file = VLC.GetVLCSettingsPath();

            //msg = null;
            if (!File.Exists(file))
            {
                //msg = $"VLC config file not at expected location {file}";
                throw new Exception($"VLC config file not at expected location {file}");
                return;
            }

            // backup current version of file // todo
            var destFile = $"{file}_audiosort_bak";

            if (!File.Exists(destFile))
                File.Copy(file, destFile);

            const char LF = '\n'; // by observation config file uses \n by itself for line breaks

            string line = null;
            var newContent = new StringBuilder();

            // loop through content and write a line to the new file'
            using (var reader = new StreamReader(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    Pref opt;
                    if (Pref.TryParse(line, out opt) && _vlcOptions.Any(n => opt.IsOption(n)) && settings.ApplyToSetting(opt))
                    {
                        if (!opt.IsEnabled)
                            throw new Exception("invalid setting"); // values already checked, something is wrong parser 

                        // write setting  to new content 
                        newContent.Append(opt);
                        newContent.Append(LF);
                        continue;
                    }

                    // any non-setting line or non-relevant setting gets repeated as is
                    // write line to new content  
                    newContent.Append(line);
                    newContent.Append(LF);
                }
            }

            // save updated content to file // todo
            var tmp = Path.GetTempFileName();
            File.WriteAllText(tmp, newContent.ToString());

            File.Copy(tmp, file, true);
            File.Delete(tmp);
        }

    }
}
