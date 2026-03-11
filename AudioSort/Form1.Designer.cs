namespace AudioSort
{
    partial class Form1
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
            this.btnStart = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFolder1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFolder2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect1 = new System.Windows.Forms.Button();
            this.btnSelect2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miVLCSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.miPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(639, 105);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "connect";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 168);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(726, 25);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 20);
            this.toolStripStatusLabel1.Text = "label";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.txtFolder1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtFolder2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnSelect1);
            this.panel1.Controls.Add(this.btnSelect2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(726, 140);
            this.panel1.TabIndex = 1;
            // 
            // txtFolder1
            // 
            this.txtFolder1.Location = new System.Drawing.Point(191, 12);
            this.txtFolder1.Name = "txtFolder1";
            this.txtFolder1.Size = new System.Drawing.Size(523, 22);
            this.txtFolder1.TabIndex = 2;
            this.txtFolder1.Leave += new System.EventHandler(this.txtCrateFolder_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Ctrl+Shift+2";
            // 
            // txtFolder2
            // 
            this.txtFolder2.Location = new System.Drawing.Point(191, 41);
            this.txtFolder2.Name = "txtFolder2";
            this.txtFolder2.Size = new System.Drawing.Size(523, 22);
            this.txtFolder2.TabIndex = 5;
            this.txtFolder2.Leave += new System.EventHandler(this.txtCrateFolder_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ctrl+Shift+1";
            // 
            // btnSelect1
            // 
            this.btnSelect1.Location = new System.Drawing.Point(110, 12);
            this.btnSelect1.Name = "btnSelect1";
            this.btnSelect1.Size = new System.Drawing.Size(75, 23);
            this.btnSelect1.TabIndex = 1;
            this.btnSelect1.Text = "select";
            this.btnSelect1.UseVisualStyleBackColor = true;
            this.btnSelect1.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // btnSelect2
            // 
            this.btnSelect2.Location = new System.Drawing.Point(110, 41);
            this.btnSelect2.Name = "btnSelect2";
            this.btnSelect2.Size = new System.Drawing.Size(75, 23);
            this.btnSelect2.TabIndex = 4;
            this.btnSelect2.Text = "select";
            this.btnSelect2.UseVisualStyleBackColor = true;
            this.btnSelect2.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(726, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miVLCSetup,
            this.miPreferences});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.optionsToolStripMenuItem.Text = "&options";
            // 
            // miVLCSetup
            // 
            this.miVLCSetup.Name = "miVLCSetup";
            this.miVLCSetup.Size = new System.Drawing.Size(161, 26);
            this.miVLCSetup.Text = "&VLC setup";
            this.miVLCSetup.Click += new System.EventHandler(this.miVLCSetup_Click);
            // 
            // miPreferences
            // 
            this.miPreferences.Name = "miPreferences";
            this.miPreferences.Size = new System.Drawing.Size(161, 26);
            this.miPreferences.Text = "&preferences";
            this.miPreferences.Click += new System.EventHandler(this.miPreferences_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 193);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AudioSort";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtFolder1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFolder2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect1;
        private System.Windows.Forms.Button btnSelect2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miVLCSetup;
        private System.Windows.Forms.ToolStripMenuItem miPreferences;
    }
}

