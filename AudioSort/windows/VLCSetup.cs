using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AudioSort
{
    public partial class VLCSetup : Form
    {
        public string _info
        {
            //get { return this.toolStripStatusLabel1.Text; }
            set { this.toolStripStatusLabel1.Text = value; }
        }

        public VLCSetup()
        {
            InitializeComponent();
            this.numHttpPort.Controls[0].Hide(); // hide arrows (leaves blank space but its good enough for this)
        }

        private void VLCSetup_Load(object sender, EventArgs e)
        {
            _info = string.Empty;
            btnSave.Focus();

            //   display current settings

            var settings = new VLC.Preferences();
            settings.LoadConfig();

            this.txtVLCPath.Text = settings.VLCPath;
            if (string.IsNullOrWhiteSpace(this.txtVLCPath.Text))
                this.txtVLCPath.Text = VLC.GetProcessPath();

            this.txtVlcrc.Text = settings.VLCConfigPath;
            this.txtVlcrc.SetPlaceholder(Util.TXT_DEFAULT);

            this.chkAutoStart.Checked = settings.AutoStart;
            this.numHttpPort.Value = settings.Port;
            this.txtHttpPassword.Text = settings.Password;
        }

        private void btnVLCPath_Click(object sender, EventArgs e)
        {
            string src;
            if (Util.UserSelectFile(Util.VLC_EXE, txtVLCPath.Text, out src))
            {
                txtVLCPath.Text = src.Trim();
            }
        }

        private void btnVlcrc_Click(object sender, EventArgs e)
        {
            string startIn = txtVlcrc.Text;
            if (string.IsNullOrWhiteSpace(startIn))
                startIn = VLC.GetVLCSettingsPath();

            string src;
            if (Util.UserSelectFile(Util.VLC_CONFIG, startIn, out src))
            {
                txtVlcrc.Text = src.Trim();
            }
        }

        private void txtVLCPath_Leave(object sender, EventArgs e)
        {
            var path = txtVLCPath.Text.Trim();
            if (!string.IsNullOrWhiteSpace(path) && !System.IO.File.Exists(path))
            {
                _info = $"vlc.exe does not exist at the given path";
            }
        }

        private void txtVlcrc_Leave(object sender, EventArgs e)
        {
            var path = txtVlcrc.Text.Trim();
            if (!string.IsNullOrWhiteSpace(path) && !System.IO.File.Exists(path))
            {
                _info = $"vlcrc does not exist at the given path";
            }
        }

        /// <summary>
        /// read values from VLC's configuration file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadVLC_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                var settings = VLC.ReadVLCConfigFile(out msg);
                if (settings == null)
                    throw new Exception(msg);

                this.numHttpPort.Value = settings.Port;
                this.txtHttpPassword.Text = settings.Password;

                var ishttp = settings.HttpEnabled ? "is" : "not";
                _info = $"http {ishttp} enabled";
            }
            catch (Exception ex)
            {
                _info = ex.Message;
            }
        }

        /// <summary>
        /// modify VLC's configuration file to enable the http server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteVLC_Click(object sender, EventArgs e)
        {
            var settings = new VLC.Preferences();
            settings.LoadConfig();

            settings.HttpEnabled = true;
            settings.Port = (int)numHttpPort.Value;
            settings.Password = txtHttpPassword.Text.Trim();
            // todo host, maybe

            string msg;
            if (!settings.ValidateSettings(out msg))
            {
                _info = $"configuration incorrect for http: {msg}";
                return;
            }

            try
            {
                VLC.WriteVLCConfigFile(settings);
                _info = $"VLC config modified, reboot VLC.";

                VLC.Close();
            }
            catch (Exception ex)
            {
                _info = $"VLC config write failed: {ex.Message}";
            }
        }

        /// <summary>
        /// write vlc setup values to app.config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            var newSettings = new VLC.Preferences()
            {
                VLCPath = txtVLCPath.Text.Trim(),
                VLCConfigPath = txtVlcrc.Text.Trim(),
                AutoStart = chkAutoStart.Checked,
                Port = (int)numHttpPort.Value,
                Password = txtHttpPassword.Text.Trim(),
            };

            string msg;
            if (!newSettings.ValidateSettings(out msg))
            {
                _info = $"configuration incorrect for http: {msg}";
                return;
            }

            var curSettings = new VLC.Preferences();
            curSettings.LoadConfig();

            var data = new Dictionary<string, string>();

            if (Util.ValueChanged(newSettings.VLCPath, curSettings.VLCPath))
            {
                data.Add(AppConfig.VLC_PATH, newSettings.VLCPath);
            }

            if (Util.ValueChanged(newSettings.VLCConfigPath, curSettings.VLCConfigPath))
            {
                data.Add(AppConfig.VLC_CONFIG_PATH, newSettings.VLCConfigPath);
            }

            if (newSettings.AutoStart != curSettings.AutoStart)
            {
                data.Add(AppConfig.AUTOSTART, newSettings.AutoStart.ToString().ToLowerInvariant()); // todo verify lowercase parses back ok
            }

            if (newSettings.Port != curSettings.Port)
            {
                data.Add(AppConfig.PORT, Convert.ToString(newSettings.Port));
            }

            if (Util.ValueChanged(newSettings.Password, curSettings.Password))
            {
                data.Add(AppConfig.PASSWORD, newSettings.Password);
            }

            if (data.Count > 0)
            {
                AppConfig.UpdateConfigFile(data);
                _info = "config file updated";

                Util.OnConfigUpdated(this, new ConfigUpdatedEventArgs(data));
            }
            else
            {
                _info = "all values match saved values, no action performed";
            }
        }
    }
}
