using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioSort
{
    public partial class AppPrefs : Form
    {
        public string _info
        {
            set { this.toolStripStatusLabel1.Text = value; }
        }

        public AppPrefs()
        {
            InitializeComponent();
        }

        private void AppPrefs_Load(object sender, EventArgs e)
        {
            _info = string.Empty;
            btnSave.Focus();

            //   load custom sound paths

            txtConfirmSound.Text = AppConfig.confirmSound;
            txtConfirmSound.SetPlaceholder(Util.TXT_DEFAULT);

            txtProblemSound.Text = AppConfig.problemSound;
            txtProblemSound.SetPlaceholder(Util.TXT_DEFAULT);

            // todo dynamic crates and mod keys controls
        }

        private void btnConfirmSelect_Click(object sender, EventArgs e)
        {
            string src;
            if (Util.UserSelectFile(Util.WAVE, txtConfirmSound.Text, out src))
            {
                txtConfirmSound.Text = src.Trim();
            }
        }

        private void btnProblemSelect_Click(object sender, EventArgs e)
        {
            string src;
            if (Util.UserSelectFile(Util.WAVE, txtProblemSound.Text, out src))
            {
                txtProblemSound.Text = src.Trim();
            }
        }

        private void txt_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void txt_DragDrop(object sender, DragEventArgs e)
        {
            var s = Util.GetDropText(e);
            if (!string.IsNullOrWhiteSpace(s))
            {
                var input = (TextBox)sender;
                input.Text = s;
            }
        }

        private void btnConfirmPlay_Click(object sender, EventArgs e)
        {
            var player = Util.confirm_default;

            string src = txtConfirmSound.Text.Trim();
            if (!string.IsNullOrWhiteSpace(src))
            {
                try
                {
                    player = Util.GetSound(src);
                }
                catch (Exception ex)
                {
                    _info = $"couldn't load sound file: {ex.Message}";
                    return;
                }
            }

            if (!player.IsLoadCompleted)
                player.Load();

            player.Play();
        }

        private void btnProblemPlay_Click(object sender, EventArgs e)
        {
            var player = Util.problem_default;

            string src = txtProblemSound.Text.Trim();

            if (!string.IsNullOrWhiteSpace(src))
            {
                try
                {
                    player = Util.GetSound(src);
                }
                catch (Exception ex)
                {
                    _info = $"couldn't load sound file: {ex.Message}";
                    return;
                }
            }

            if (!player.IsLoadCompleted)
                player.Load();

            player.Play();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var confirmFile = txtConfirmSound.Text.Trim();
            var problemFile = txtProblemSound.Text.Trim();

            var data = new Dictionary<string, string>();

            if (!string.Equals(confirmFile, AppConfig.confirmSound, StringComparison.InvariantCultureIgnoreCase))
            {
                data.Add(AppConfig.CONFIRM_SOUND, confirmFile);
            }

            if (!string.Equals(problemFile, AppConfig.problemSound, StringComparison.InvariantCultureIgnoreCase))
            {
                data.Add(AppConfig.PROBLEM_SOUND, problemFile);
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
