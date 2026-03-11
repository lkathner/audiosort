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
        private HockeyManager _hkm;
        private List<int> _registeredIDs;
        private CrateList _activeCrates;

        private Playlist _playlist;
        private Timer _timer;

        public string _info
        {
            //get { return this.toolStripStatusLabel1.Text; }
            set { this.toolStripStatusLabel1.Text = value; }
        }

        public Form1()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.LP;

        }

        ~Form1()
        {
            // HockeyManager.UnregisterHotKeys(_registeredIDs); // does not get called?

        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _info = "loading...";
            btnStart.Focus();

            this.btnStart.Enabled = false;

            _hkm = new HockeyManager(this);
            _registeredIDs = new List<int>();

            _timer = new Timer();
            _timer.Tick += _timer_Tick;

            Util.ConfigUpdated += Wnd_ConfigUpdated;

            /* todo remove after commit
            this.txtFolder1.Text = ConfigurationManager.AppSettings["txtFolder1"];
            this.txtFolder2.Text = ConfigurationManager.AppSettings["txtFolder2"];

            var port = ConfigurationManager.AppSettings["port"];
            var user = ConfigurationManager.AppSettings["user"];
            var password = ConfigurationManager.AppSettings["password"];

            VLC.Set(port, user, password);
            */

            try
            {
                LoadSettings();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, "couldn't load saved settings");
                return; // fatal error, user won't be able to fix
            }

            try
            {
                EnableHotKeys();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, "Couldn't attach hotkeys?");
                return;
            }

            if (VLC.Settings.AutoStart)
            {
                try
                {
                    VLC.Launch();
                }
                catch (Exception ex)
                {
                    _info = $"couldn't start VLC: {ex.Message}";
                    return;
                }
            }

            try
            {
                RefreshPlaylist();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get playlist and it exploded");
                return;
            }

            try
            {
                RefreshStatus();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get status and it exploded");
                return;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _hkm.UnregisterHotKeys(_registeredIDs);

            base.OnClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == HockeyManager.WM_HOTKEY)
            {
                var e = new HotKeyEventArgs(m.LParam);
                HockeyManager.OnHotKeyPressed(e);
            }

            base.WndProc(ref m);
        }

        #region events

        /* todo remove after commit
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            // TODO this is the playlist button, not currently implemented

            var ofd = new OpenFileDialog
            {
                //ValidateNames = false,
                //CheckFileExists = false
            };

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    this.txtSourceFolder.Text = System.IO.Path.GetDirectoryName(ofd.FileName);
            //}
        }
        */

        private void miVLCSetup_Click(object sender, EventArgs e)
        {
            ShowSettingsWnd();
        }

        private void miPreferences_Click(object sender, EventArgs e)
        {
            ShowPreferencesWnd();
        }

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int crateID = (int)btn.Tag;
            var crate = _activeCrates.FirstOrDefault(n => n.ID == crateID);

            if (crate == null)
                throw new Exception($"invalid crate button tag {btn.Tag}");

            string path;
            if (Util.UserSelectFolder(crate.InputControl.Text, out path))
            {
                crate.InputControl.Text = path;
                UpdateCratePath(crate, path);
            }
        }

        /* todo remove after commit
        private void btnSelect2_Click(object sender, EventArgs e)
        {
            var input = txtFolder2;

        }
        */

        private void txtCrateFolder_Leave(object sender, EventArgs e)
        {
            var input = (TextBox)sender;
            int crateID = (int)input.Tag;
            var path = !string.IsNullOrWhiteSpace(input.Text)
                ? input.Text.Trim()
                : null;

            var crate = _activeCrates.FirstOrDefault(n => n.ID == crateID);
            if (crate == null)
                throw new Exception($"invalid crate input tag {input.Tag}");

            UpdateCratePath(crate, path);
        }


        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                RefreshStatus();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get status and it exploded");
            }

        }

        /* todo remove after commit
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = string.Empty;
        }
        */

        private void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                RefreshStatus();
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, @"tried to get status and it exploded");
            }
        }

        private void HockeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
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

        private void Wnd_ConfigUpdated(object sender, ConfigUpdatedEventArgs e)
        {
            // refresh values from config
            try
            {
                LoadSettings();
                _info = $"reloaded settings";
            }
            catch (Exception ex)
            {
                ShowBrokeIt(ex, "couldn't load saved settings");
            }
        }

        #endregion


        #region support methods

        private void LoadSettings()
        {
            btnSelect1.Tag = 1;
            btnSelect2.Tag = 2;

            _activeCrates = new CrateList();
            _activeCrates.Add(0, new TextBox()); // for testing
            _activeCrates.Add(1, txtFolder1);
            _activeCrates.Add(2, txtFolder2);

            if (!VLC.LoadConfig())
            {
                ShowSettingsWnd();
            }
        }

        private void ShowSettingsWnd()
        {
            var wnd = new VLCSetup();
            if (this.OwnedForms.Any(n => string.Equals(n.Name, wnd.Name)))
            {
                return;
            }

            wnd.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y);
            wnd.Show(this);
        }

        private void ShowPreferencesWnd()
        {
            var wnd = new AppPrefs();
            if (this.OwnedForms.Any(n => string.Equals(n.Name, wnd.Name)))
            {
                return;
            }

            wnd.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height);
            wnd.Show(this);
        }

        private void ShowBrokeIt(Exception ex, string msgmessage = null)
        {
            Util.ShowBrokeItWnd(ex, msgmessage);
            return;

            // TODO clean up redundant calls to this
            // should only get called when something unexpected happens

            var s = "what are you doing, stop it";
            try
            {
                _info = s;

                var msg = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(msgmessage))
                {
                    _info = msgmessage;

                    msg.AppendLine(msgmessage);
                    msg.AppendLine();
                }

                //msg.Append(m2);
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
                frm.Show();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, s);
            }
        }

        /// <summary>
        /// get current media and activity from vlc
        /// </summary>
        private async void RefreshStatus()
        {
            this.btnStart.Enabled = false;

            if (_timer.Enabled)
                _timer.Stop();

            try
            {
                var vlcStatus = await VLC.GetCurrentStatus();
                _info = $"[{vlcStatus.State}] {vlcStatus.CurrentSong}".Trim();

                if (vlcStatus.IsPlaying)
                {
                    var tremain = vlcStatus.Length - vlcStatus.Time; // in seconds
                    _timer.Interval = (tremain + 2) * 1000; // ms
                    _timer.Start();
                }
            }
            catch (Exception ex)
            {
                _info = $"Couldn't get status from VLC: {ex.Message}";
            }

            this.btnStart.Enabled = true;
        }

        /// <summary>
        /// get current playlist items from vlc
        /// </summary>
        private async void RefreshPlaylist()
        {
            this.btnStart.Enabled = false;
            _playlist = null;

            try
            {
                _playlist = await VLC.GetPlaylistInfo();
                _info = $"Retrieved playlist, {_playlist.Songs.Count} items";
            }
            catch (Exception ex)
            {
                _info = $"Couldn't get playlist: {ex.Message}";
            }

            this.btnStart.Enabled = true;
        }

        /// <summary>
        /// register hotkeys with the system
        /// </summary>
        private void EnableHotKeys()
        {
            var mod = KeyModifiers.Control | KeyModifiers.Shift;

            HockeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HockeyManager_HotKeyPressed);

            foreach (var crate in _activeCrates)
            {
                foreach (var k in crate.KBKeys)
                {
                    int d = _hkm.RegisterHotKey(k, mod);
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


        /// <summary>
        /// determine what to do when a hotkey is pressed
        /// </summary>
        /// <param name="e"></param>
        private async void HandleHotkeys(HotKeyEventArgs e)
        {
            var mod = KeyModifiers.Control | KeyModifiers.Shift;

            //            https://wiki.videolan.org/VLC_HTTP_requests/

            if (!e.Modifiers.HasFlag(mod))
            {
                return;
            }

            Status status;
            var result = CopyResult.None;

            try
            {
                status = await VLC.GetCurrentStatus();
            }
            catch (Exception ex)
            {
                _info = $"Could not get status from VLC: {ex.Message}";
                return;
            }

            if (string.IsNullOrWhiteSpace(status.CurrentSong))
            {
                _info = $"No song info available (currently {status.State}).";
                return;
            }

            var crate = _activeCrates.FindByPressedKey(e.Key);
            if (crate != null)
            {
                result = CopyToCrate(status, crate);
            }
            else // for testing
            {
                result = CopyResult.Problem;
                ShowBrokeIt(new Exception("crate 0 hotkey test"));
            }

            Util.PlayStatusSound(result);
        }


        /* todo remove after commit
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
        */

        private CopyResult CopyToCrate(Status status, Crate crate)
        {
            var fresh = false;

            if (_playlist == null)
            {
                RefreshPlaylist();
                fresh = true;

                if (_playlist == null)
                {
                    // no playlist available, can't do anything
                    // RefreshPlaylist sets status text, do not change
                    return CopyResult.Problem;
                }
            }

            var song = _playlist.Pick(status);
            if (song == null && !fresh)
            {
                RefreshPlaylist();
                fresh = true;

                if (_playlist != null)
                    song = _playlist.Pick(status);
            }

            if (song == null)
            {
                _info = $"Could not match file to song {status.CurrentSong}";
                return CopyResult.Problem;
            }

            // --- todo move up when reorganizing
            string folder = crate.FolderPath;

            if (string.IsNullOrWhiteSpace(folder))
            {
                _info = $"No destination folder name provided.";
                return CopyResult.Problem;
            }

            try
            {
                if (!Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                _info = $"Folder [{folder}] doesn't exist and couldn't be created: {ex.Message}.";
                return CopyResult.Problem;
            }
            // ---

            string dest = System.IO.Path.Combine(folder, song.Filename);
            if (System.IO.File.Exists(dest))
            {
                var fiNew = new FileInfo(song.Location);
                var fiExists = new FileInfo(dest);

                if (fiNew.Length != fiExists.Length)
                {
                    _info = $"Existing file conflict at {dest}";
                    return CopyResult.Problem;
                }

                // assume same file
                _info = $"File {song.Filename} already in crate, skipping";
                return CopyResult.None;
            }

            try
            {
                System.IO.File.Copy(song.Location, dest);
                _info = $"File copied to {dest}";
                return CopyResult.Success;
            }
            catch (Exception ex)
            {
                _info = $"File copy failed: {ex.Message}";
                return CopyResult.Problem;
            }

        }


        /* todo remove after commit
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
        */

        private void UpdateCratePath(Crate crate, string path)
        {
            if (path != null)
            {
                var current = crate.FolderPath;
                if (string.Equals(current, path, StringComparison.InvariantCultureIgnoreCase))
                {
                    return; // same path, nothing to change
                }

                crate.FolderPath = path;
            }
            else // path null
            {
                // todo sort this out if dynamic crates implemented
                // -----------
                //if (!_crates2.Remove(crate))
                //    return; // no crate path saved, nothing to change
                // -----------
                crate.FolderPath = string.Empty;
                // -----------
            }

            var data = new Dictionary<string, string>();
            data.Add(crate.ConfigKey, crate.FolderPath);

            // todo rename txt inputs, add null entry to data to remove old key

            AppConfig.UpdateConfigFile(data);
            _info = "config file updated";
        }

        #endregion

    }
}
