namespace AudioSort
{
    partial class VLCSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReadVLC = new System.Windows.Forms.Button();
            this.btnVLCPath = new System.Windows.Forms.Button();
            this.txtVLCPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnWriteVLC = new System.Windows.Forms.Button();
            this.txtHttpPassword = new System.Windows.Forms.TextBox();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.numHttpPort = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnVlcrc = new System.Windows.Forms.Button();
            this.txtVlcrc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numHttpPort)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "port";
            // 
            // btnReadVLC
            // 
            this.btnReadVLC.Location = new System.Drawing.Point(12, 133);
            this.btnReadVLC.Name = "btnReadVLC";
            this.btnReadVLC.Size = new System.Drawing.Size(137, 23);
            this.btnReadVLC.TabIndex = 7;
            this.btnReadVLC.Text = "read VLC config";
            this.toolTip1.SetToolTip(this.btnReadVLC, "Copy current settings from VLC config and save them here");
            this.btnReadVLC.UseVisualStyleBackColor = true;
            this.btnReadVLC.Click += new System.EventHandler(this.btnReadVLC_Click);
            // 
            // btnVLCPath
            // 
            this.btnVLCPath.Location = new System.Drawing.Point(12, 68);
            this.btnVLCPath.Name = "btnVLCPath";
            this.btnVLCPath.Size = new System.Drawing.Size(87, 23);
            this.btnVLCPath.TabIndex = 0;
            this.btnVLCPath.Text = "VLC path";
            this.btnVLCPath.UseVisualStyleBackColor = true;
            this.btnVLCPath.Click += new System.EventHandler(this.btnVLCPath_Click);
            // 
            // txtVLCPath
            // 
            this.txtVLCPath.Location = new System.Drawing.Point(105, 68);
            this.txtVLCPath.Name = "txtVLCPath";
            this.txtVLCPath.Size = new System.Drawing.Size(345, 22);
            this.txtVLCPath.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtVLCPath, "Path the the VLC executable. If VLC is found running, that path will be filled au" +
        "tomatically. The path can be provided manually if it\'s not found or another vers" +
        "ion should be the default.");
            this.txtVLCPath.Leave += new System.EventHandler(this.txtVLCPath_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "password";
            // 
            // btnWriteVLC
            // 
            this.btnWriteVLC.Location = new System.Drawing.Point(155, 133);
            this.btnWriteVLC.Name = "btnWriteVLC";
            this.btnWriteVLC.Size = new System.Drawing.Size(137, 23);
            this.btnWriteVLC.TabIndex = 8;
            this.btnWriteVLC.Text = "enable VLC http";
            this.toolTip1.SetToolTip(this.btnWriteVLC, "Modify VLC config to enable the http server using the current settings");
            this.btnWriteVLC.UseVisualStyleBackColor = true;
            this.btnWriteVLC.Click += new System.EventHandler(this.btnWriteVLC_Click);
            // 
            // txtHttpPassword
            // 
            this.txtHttpPassword.Location = new System.Drawing.Point(105, 40);
            this.txtHttpPassword.Name = "txtHttpPassword";
            this.txtHttpPassword.Size = new System.Drawing.Size(128, 22);
            this.txtHttpPassword.TabIndex = 6;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(456, 70);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(90, 21);
            this.chkAutoStart.TabIndex = 2;
            this.chkAutoStart.Text = "auto start";
            this.toolTip1.SetToolTip(this.chkAutoStart, "Launch VLC at startup if not already running");
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // numHttpPort
            // 
            this.numHttpPort.Location = new System.Drawing.Point(105, 12);
            this.numHttpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numHttpPort.Name = "numHttpPort";
            this.numHttpPort.Size = new System.Drawing.Size(128, 22);
            this.numHttpPort.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 168);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(558, 25);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 20);
            this.toolStripStatusLabel1.Text = "label";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(471, 133);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnVlcrc
            // 
            this.btnVlcrc.Location = new System.Drawing.Point(12, 96);
            this.btnVlcrc.Name = "btnVlcrc";
            this.btnVlcrc.Size = new System.Drawing.Size(87, 23);
            this.btnVlcrc.TabIndex = 11;
            this.btnVlcrc.Text = "vlcrc";
            this.btnVlcrc.UseVisualStyleBackColor = true;
            this.btnVlcrc.Click += new System.EventHandler(this.btnVlcrc_Click);
            // 
            // txtVlcrc
            // 
            this.txtVlcrc.Location = new System.Drawing.Point(105, 96);
            this.txtVlcrc.Name = "txtVlcrc";
            this.txtVlcrc.Size = new System.Drawing.Size(345, 22);
            this.txtVlcrc.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtVlcrc, "Path to VLC config file. The default appdata path is assumed when not specified.");
            this.txtVlcrc.Leave += new System.EventHandler(this.txtVlcrc_Leave);
            // 
            // VLCSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 193);
            this.Controls.Add(this.btnVlcrc);
            this.Controls.Add(this.txtVlcrc);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.numHttpPort);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWriteVLC);
            this.Controls.Add(this.txtHttpPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReadVLC);
            this.Controls.Add(this.btnVLCPath);
            this.Controls.Add(this.txtVLCPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VLCSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "VLC Setup";
            this.Load += new System.EventHandler(this.VLCSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numHttpPort)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReadVLC;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnVLCPath;
        private System.Windows.Forms.TextBox txtVLCPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnWriteVLC;
        private System.Windows.Forms.TextBox txtHttpPassword;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.NumericUpDown numHttpPort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnVlcrc;
        private System.Windows.Forms.TextBox txtVlcrc;
    }
}