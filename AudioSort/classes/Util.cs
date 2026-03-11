using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace AudioSort
{
    public static class Util
    {
        static Util()
        {
            // load sound samples

            confirm_default = new System.Media.SoundPlayer(Properties.Resources.confirm_2);
            confirm_default.LoadAsync();

            problem_default = new System.Media.SoundPlayer(Properties.Resources.problem_2);
            problem_default.LoadAsync();

            if (!string.IsNullOrWhiteSpace(AppConfig.confirmSound))
            {
                confirm_custom = GetSound(AppConfig.confirmSound);
            }

            if (!string.IsNullOrWhiteSpace(AppConfig.problemSound))
            {
                problem_custom = GetSound(AppConfig.problemSound);
            }
        }

        #region pinvokes

        private const uint EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint SendMessage(IntPtr hWnd, uint msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        //private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        #endregion

        #region UI interactions

        // file select filters
        public const string VLC_EXE = "VLC executable|vlc.exe";
        public const string VLC_CONFIG = "VLC config|vlcrc";
        public const string WAVE = "wav audio|*.wav";

        // standard input placeholder text
        public const string TXT_DEFAULT = "(default)";

        public static event EventHandler<ConfigUpdatedEventArgs> ConfigUpdated;

        internal static void ShowBrokeItWnd(Exception ex, string msgmessage = null)
        {
            // TODO clean up redundant calls to this
            // should only get called when something unexpected happens

            var s = "what are you doing, stop it";
            try
            {
                var msg = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(msgmessage))
                {
                    msg.AppendLine(msgmessage);
                    msg.AppendLine();
                }

                if (ex != null)
                {
                    msg.AppendLine(ex.Message);
                }

                var frm = new BrokeIt
                {
                    //Width = 498,
                    //Height = 280 + 80
                };

                frm.SetMessage(msg.ToString());
                frm.TopMost = true;
                frm.Show();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, s);
            }
        }

        public static bool UserSelectFile(string filterExt, string startIn, out string path)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "select file",
            };

            if (!string.IsNullOrWhiteSpace(filterExt))
            {
                ofd.Filter = filterExt;
            }

            if (!string.IsNullOrWhiteSpace(startIn))
            {
                try
                {
                    var dir = Path.GetDirectoryName(startIn);
                    if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                    {
                        ofd.InitialDirectory = dir;
                    }
                    else
                    {
                        throw new Exception($"directory '{dir}' invalid");
                    }
                }
                catch (Exception ex)
                {
                    // todo maybe log somewhere
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                try
                {
                    var fn = Path.GetFileName(startIn);
                    if (!string.IsNullOrWhiteSpace(fn))
                    {
                        ofd.FileName = fn;
                    }
                }
                catch (Exception ex)
                {
                    // todo maybe log somewhere
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                return true;
            }

            path = string.Empty;
            return false;
        }

        public static bool UserSelectFolder(string startIn, out string path)
        {
            // TODO that win32 folder select dialog

            var ofd = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                FileName = "select folder",
            };

            if (!string.IsNullOrWhiteSpace(startIn))
            {
                try
                {
                    ofd.InitialDirectory = (startIn);
                    if (!Directory.Exists(ofd.InitialDirectory))
                    {
                        ofd.InitialDirectory = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    // todo maybe log somewhere
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = Path.GetDirectoryName(ofd.FileName);
                return true;
            }

            path = string.Empty;
            return false;
        }

        /// <summary>
        /// gets text for dropped file or text selection
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static string GetDropText(DragEventArgs e)
        {
            var dropped = string.Empty;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    dropped = files[0]; // todo probably should return multiple file names if given
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                dropped = (string)e.Data.GetData(DataFormats.Text);
            }

            return dropped;
        }

        /// <summary>
        /// set and show placeholder text on a textbox control. visible when control is empty.
        /// </summary>
        /// <param name="textBox">TextBox control</param>
        /// <param name="text">placeholder message</param>
        public static void SetPlaceholder(this TextBox textBox, string text)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, text);
        }

        public static void OnConfigUpdated(object sender, ConfigUpdatedEventArgs e)
        {
            if (ConfigUpdated != null)
            {
                ConfigUpdated(sender, e);
            }
        }

        #endregion

        #region sound stuff

        // sound players kept active in memory
        public static readonly System.Media.SoundPlayer confirm_default;
        public static readonly System.Media.SoundPlayer problem_default;
        public static System.Media.SoundPlayer confirm_custom;
        public static System.Media.SoundPlayer problem_custom;

        /// <summary>
        /// load a wav file for playing
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static System.Media.SoundPlayer GetSound(string src)
        {
            var player = new System.Media.SoundPlayer(src);
            player.LoadAsync();
            return player;
        }

        /// <summary>
        /// get the configured success sound player
        /// </summary>
        /// <returns></returns>
        internal static System.Media.SoundPlayer GetSuccessSound()
        {
            var player = confirm_default;

            if (confirm_custom != null)
            {
                player = confirm_custom; // custom sound if in use
            }

            return player;
        }

        /// <summary>
        /// get the configured problem sound player
        /// </summary>
        /// <returns></returns>
        internal static System.Media.SoundPlayer GetProblemSound()
        {
            var player = problem_default;

            if (problem_custom != null) // custom sound if in use
            {
                player = problem_custom;
            }

            return player;
        }

        internal static void PlayStatusSound(CopyResult result)
        {
            System.Media.SoundPlayer player = null;

            switch (result)
            {
                case CopyResult.Success:
                    player = GetSuccessSound();
                    break;
                case CopyResult.Problem:
                    player = GetProblemSound();
                    break;
            }

            if (player != null)
            {
                if (!player.IsLoadCompleted)
                    player.Load();

                player.Play();
            }
        }

        #endregion

        internal static string GetConfigLineEndings(string file)
        {
            const char CR = '\r';
            const char LF = '\n';

            return LF.ToString();

            string nl = string.Empty;

            using (var reader = new StreamReader(file))
            {
                int i;
                while ((i = reader.Read()) != -1)
                {
                    char c = (char)i;

                    if (c == CR)
                    {
                        nl = CR.ToString();

                        c = (char)reader.Peek();
                        if (c == LF)
                            nl = $"{CR}{LF}";

                        break;
                    }

                    if (c == LF)
                    {
                        nl = LF.ToString();

                        c = (char)reader.Peek();
                        if (c == CR)
                        {
                            // this is backwards // todo
                        }

                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(nl))
                return nl;

            return Environment.NewLine;
        }

        /// <summary>
        /// test if a specifc value has changed between instances
        /// </summary>
        /// <param name="newstr"></param>
        /// <param name="oldstr"></param>
        /// <returns></returns>
        internal static bool ValueChanged(string newstr, string oldstr)
        {
            // null and empty are functionally the same
            newstr = newstr?.Trim() ?? string.Empty;
            oldstr = oldstr?.Trim() ?? string.Empty;

            var same = string.Equals(newstr, oldstr, StringComparison.InvariantCultureIgnoreCase);
            return !same;
        }
    }

    public class ConfigUpdatedEventArgs : EventArgs
    {
        public Dictionary<string, string> UpdatedValues { get; set; }

        public ConfigUpdatedEventArgs(Dictionary<string, string> data)
        {
            this.UpdatedValues = data;
        }
    }

    public enum CopyResult
    {
        None,
        Success,
        Problem,
    }
}
