using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace AudioSort
{
    public partial class Form1 : Form
    {
        private const int WM_HOTKEY = 0x312;
        private List<int> _registeredIDs;
        Dictionary<int, List<Keys>> _activeKeys;

        private Playlist _playlist;


        public Form1()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.LP;

        }

        ~Form1()
        {
            HotKeyManager.UnregisterHotKey(_registeredIDs);
            //HotKeyManager.RegisterHotKey( this, Keys.A, KeyModifiers.Alt);

        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _registeredIDs = new List<int>();

            this.txtFolder1.Text = ConfigurationManager.AppSettings["txtFolder1"];
            this.txtFolder2.Text = ConfigurationManager.AppSettings["txtFolder2"];

            var port = ConfigurationManager.AppSettings["port"];
            var user = ConfigurationManager.AppSettings["user"];
            var password = ConfigurationManager.AppSettings["password"];

            VLC.Set(port, user, password);

            try
            {
                EnableHotKeys();
            }
            catch (Exception ex)
            {
                var msg2 = "Couldn't register hotkeys?";
                ShowBrokeIt(ex, msg2);

                this.btnStart.Enabled = false;
                return;
            }

            try
            {
                RefreshPlaylist();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get playlist and it exploded");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            HotKeyManager.UnregisterHotKey(_registeredIDs);

            base.OnClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                var e = new HotKeyEventArgs(m.LParam);
                HotKeyManager.OnHotKeyPressed(e);
            }

            base.WndProc(ref m);
        }


        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtSourceFolder.Text = System.IO.Path.GetDirectoryName(ofd.FileName);
            }
        }


        private void btnSelect1_Click(object sender, EventArgs e)
        {
            this.txtFolder1.Text = SelectFolder();

            string key = "txtFolder1";
            UpdateFileSetting(key, this.txtFolder1.Text);

        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            this.txtFolder2.Text = SelectFolder();

            string key = "txtFolder2";
            UpdateFileSetting(key, this.txtFolder2.Text);
        }

        private void txtFolder1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFolder2_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                RefreshPlaylist();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get playlist and it exploded");
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = string.Empty;
        }


        void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {

            try
            {
                HandleHotkeys(e);
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, "hotkeys too hot to handle.");
            }

        }

        private void ShowBrokeIt(Exception ex, string msgmessage = null)
        {
            var s = "what are you doing, stop it";
            try
            {
                this.toolStripStatusLabel1.Text = s;

                var m2 = innerMessage(ex);
                var msg = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(msgmessage))
                {
                    this.toolStripStatusLabel1.Text = msgmessage;

                    msg.AppendLine(msgmessage);
                    msg.AppendLine();
                }

                msg.Append(m2);

                var frm = new BrokeIt
                {
                    //Width = 498,
                    //Height = 280 + 80
                };

                frm.SetMessage(m2);
                frm.ShowDialog();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, s);
            }
        }


        private void RefreshPlaylist()
        {
            try
            {
                GetPlaylistInfo();
                this.toolStripStatusLabel1.Text = "Retrieved playlist.";
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = $"Couldn't get playlist: {innerMessage(ex)}";
            }
        }

        private void GetPlaylistInfo()
        {
            var json = VLC.GetPlaylistJson();
            //System.Diagnostics.Debug.WriteLine(json);


            //var jser = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var playlist = jser.Deserialize<Playlist>(json);
            //var pl2 = jser.DeserializeObject(json);

            var pl3 = new Playlist(json);
            _playlist = pl3;
        }


        private void EnableHotKeys()
        {
            var mod = KeyModifiers.Control | KeyModifiers.Shift;

            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);

            //var _activeKeys = new Keys[0]; //{ Keys.D1, Keys.D2, Keys.NumPad1, Keys.NumPad2 };
            //var _activeKeys = new List<Keys>();
            _activeKeys = new Dictionary<int, List<Keys>>();

            _activeKeys.Add(1, new List<Keys> { Keys.D1, Keys.NumPad1, });
            _activeKeys.Add(2, new List<Keys> { Keys.D2, Keys.NumPad2, });


            foreach (var kvp in _activeKeys)
            {
                var keys = kvp.Value;

                foreach (var k in keys)
                {
                    int d = HotKeyManager.RegisterHotKey(this, k, mod);
                    _registeredIDs.Add(d);
                }
            }

            /*
            int id1 = HotKeyManager.RegisterHotKey(this, Keys.D1, KeyModifiers.Control | KeyModifiers.Shift);
            int id2 = HotKeyManager.RegisterHotKey(this, Keys.D2, mod);

            int id3 = HotKeyManager.RegisterHotKey(this, Keys.NumPad1, mod);
            int id4 = HotKeyManager.RegisterHotKey(this, Keys.NumPad2, mod);

            _registeredIDs.Add(id1);
            _registeredIDs.Add(id2);
            _registeredIDs.Add(id3);
            _registeredIDs.Add(id4);
            */

        }


        private void HandleHotkeys(HotKeyEventArgs e)
        {
            var mod = KeyModifiers.Control | KeyModifiers.Shift;

            //            https://wiki.videolan.org/VLC_HTTP_requests/

            if (_activeKeys == null)
            {
                this.toolStripStatusLabel1.Text = $"No hotkeys enabled. (how did you get here?)";
                return;
            }

            if (!e.Modifiers.HasFlag(mod))
            {
                return;
            }

            foreach (var kvp in _activeKeys)
            {
                int crateID = kvp.Key;
                var crateKeys = kvp.Value;

                if (!crateKeys.Contains(e.Key))
                {
                    continue;
                }

                var status = GetCurrentStatus();
                if (status == null)
                {
                    this.toolStripStatusLabel1.Text = $"Status unavailable (VLC not running?)";
                    return;
                }

                if (string.IsNullOrWhiteSpace(status.CurrentSong))
                {
                    this.toolStripStatusLabel1.Text = $"No song info available (currently {status.State}).";
                    return;
                }

                string folder = string.Empty;
                switch (crateID)
                {
                    case 1:
                        folder = this.txtFolder1.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(folder))
                            CopyToCrate(status, folder);
                        break;

                    case 2:
                        folder = this.txtFolder2.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(folder))
                            CopyToCrate(status, folder);
                        break;

                    default:
                        this.toolStripStatusLabel1.Text = $"Unexpected crate {crateID}, no action taken.";
                        break;
                }
            }
        }


        private Status GetCurrentStatus()
        {
            try
            {
                var json = VLC.GetStatusJson();
                var status = new Status(json);
                return status;
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = $"Couldn't get VLC status: {innerMessage(ex)}";
                return null;
            }
        }

        private string SelectFolder()
        {

            // TODO that win32 folder select dialog

            var ofd = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                FileName = "select folder",
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return System.IO.Path.GetDirectoryName(ofd.FileName);
            }

            return string.Empty;
        }

        private void CopyToCrate(Status status, string folder)
        {
            if (_playlist == null)
            {
                RefreshPlaylist();
                if (_playlist == null)
                {
                    // no playlist available, can't do anything
                    // RefreshPlaylist sets status text, do not change
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                this.toolStripStatusLabel1.Text = $"No folder name provided.";
                return;
            }

            folder = folder.Trim();
            if (!System.IO.Directory.Exists(folder))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    var s = $"Folder [{folder}] doesn't exist and couldn't be created.";
                    ShowBrokeIt(ex, s);
                    return;
                }
            }

            var song = _playlist.Pick(status);
            if (song == null)
            {
                this.toolStripStatusLabel1.Text = $"Could not match file to song {status.CurrentSong}";
                return;
            }

            try
            {
                string dest = System.IO.Path.Combine(folder, song.Filename);
                if (System.IO.File.Exists(dest))
                {
                    this.toolStripStatusLabel1.Text = $"File {song.Filename} already in crate";
                }
                else
                {
                    System.IO.File.Copy(song.Location, dest);
                    this.toolStripStatusLabel1.Text = $"File copied to {dest}";
                }
            }
            catch (Exception ex)
            {
                this.toolStripStatusLabel1.Text = $"File copy failed: {ex.Message}";
                var s = $"File copy failed?";
                ShowBrokeIt(ex, s);
            }

        }


        private void UpdateFileSetting(string key, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, text.Trim());

            config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");
        }

        private string innerMessage(Exception ex)
        {
            string msg = string.Empty;

            var x = ex;
            while (x != null)
            {
                msg = x.Message;

                if (x.HResult == -2147467259) 	  // connection refused
                {
                    msg = "VLC not running";
                    break;
                }

                x = x.InnerException;
            }

            return msg;
        }

    }
}
