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

        private Playlist _playlist;
        private FileSystemWatcher _watcher;


        public Form1()
        {
            InitializeComponent();
        }

        ~Form1()
        {
            HotKeyManager.UnregisterHotKey();
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

            this.btnStart.Visible = false;
            RefreshPlaylist();
            SetUpHotkeys();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            HotKeyManager.UnregisterHotKey();

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


        private async void W_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.FullPath);
            //this.toolStripStatusLabel1.Text = e.FullPath;

            //var progress = new Progress<string>(s => label.Text = s);
            //await Task.Factory.StartNew(() => this.toolStripStatusLabel1.Text = e.FullPath);

            string file = e.FullPath;
            lblCurrent.Invoke(new Action(() => toolStripStatusLabel1.Text = file));
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

            RefreshPlaylist();
            SetUpHotkeys();

            this.btnStart.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_watcher != null)
                _watcher.EnableRaisingEvents = false;
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
                this.toolStripStatusLabel1.Text = ex.Message;

                //MessageBox.Show(ex.Message, "blerg");
                var frm = new BrokeIt();
                frm.Width = 498;
                frm.Height = 280;
                frm.ShowDialog();

            }

        }


        private void RefreshPlaylist()
        {
            var json = VLC.GetPlaylistJson();
            //System.Diagnostics.Debug.WriteLine(json);


            var jser = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var playlist = jser.Deserialize<Playlist>(json);
            //var pl2 = jser.DeserializeObject(json);

            var pl3 = new Playlist(json);
            _playlist = pl3;

            this.toolStripStatusLabel1.Text = "Retrieved playlist.";
        }


        private void SetUpHotkeys()
        {
            var mods = KeyModifiers.Control | KeyModifiers.Shift;

            int id1 = HotKeyManager.RegisterHotKey(this, Keys.D1, KeyModifiers.Control | KeyModifiers.Shift);
            int id2 = HotKeyManager.RegisterHotKey(this, Keys.D2, KeyModifiers.Control | KeyModifiers.Shift);

            int id3 = HotKeyManager.RegisterHotKey(this, Keys.NumPad1, KeyModifiers.Control | KeyModifiers.Shift);
            int id4 = HotKeyManager.RegisterHotKey(this, Keys.NumPad2, KeyModifiers.Control | KeyModifiers.Shift);

            _registeredIDs.Add(id1);
            _registeredIDs.Add(id2);
            _registeredIDs.Add(id3);
            _registeredIDs.Add(id4);

            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
        }


        private void HandleHotkeys(HotKeyEventArgs e)
        {

            //            https://wiki.videolan.org/VLC_HTTP_requests/

            if (e.Modifiers.HasFlag(AudioSort.KeyModifiers.Control) && e.Modifiers.HasFlag(AudioSort.KeyModifiers.Shift))
            {
                var activeKeys = new[] { Keys.D1, Keys.D2, Keys.NumPad1, Keys.NumPad2 };

                if (activeKeys.Contains(e.Key))
                {
                    var json = VLC.GetStatusJson();
                    var status = new Status(json);

                    if (string.IsNullOrWhiteSpace(status.CurrentSong))
                    {
                        this.toolStripStatusLabel1.Text = $"No current song (currently {status.State}).";
                        return;
                    }

                    if (e.Key == Keys.D1 || e.Key == Keys.NumPad1)
                    {
                        var folder = this.txtFolder1.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(folder))
                            CopyFolder2(status, folder);
                    }
                    if (e.Key == Keys.D2 || e.Key == Keys.NumPad2)
                    {
                        var folder = this.txtFolder2.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(folder))
                            CopyFolder2(status, folder);
                    }
                }


            }
        }

        private string SelectFolder()
        {
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

        private void CopyFolder2(Status status, string folder)
        {

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
                    this.toolStripStatusLabel1.Text = $"Folder [{folder}] does not exist and couldn't be created.";
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
                    this.toolStripStatusLabel1.Text = $"File {song.Filename} already in folder";
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
            }

        }

        private void WatchFolder()
        {

            string path = this.txtSourceFolder.Text.Trim();
            _watcher = new FileSystemWatcher(path);

            //_watcher.NotifyFilter = System.IO.NotifyFilters.LastAccess;

            _watcher.Filter = "*.mp3|*.flac|*.mpc|*.aac";
            _watcher.Filter = "*.mp3";

            _watcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;

            _watcher.Changed += W_Changed;

            this.toolStripStatusLabel1.Text = "watcher started";
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

    }
}
