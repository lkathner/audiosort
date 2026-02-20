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
    public partial class BrokeIt : Form
    {
        public BrokeIt()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //this.pictureBox1.Width = 498;
            //this.pictureBox1.Height = 280;
        }

        private void BrokeIt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public void SetMessage(string message)
        {
            this.richTextBox1.Text = $"{message}{Environment.NewLine}{Environment.NewLine}Esc to close";
        }

    }
}
