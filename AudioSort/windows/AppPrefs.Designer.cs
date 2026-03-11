namespace AudioSort
{
    partial class AppPrefs
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
            this.btnProblemPlay = new System.Windows.Forms.Button();
            this.btnConfirmPlay = new System.Windows.Forms.Button();
            this.btnProblemSelect = new System.Windows.Forms.Button();
            this.txtProblemSound = new System.Windows.Forms.TextBox();
            this.btnConfirmSelect = new System.Windows.Forms.Button();
            this.txtConfirmSound = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numCrates = new System.Windows.Forms.NumericUpDown();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numCrates)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProblemPlay
            // 
            this.btnProblemPlay.Location = new System.Drawing.Point(495, 41);
            this.btnProblemPlay.Name = "btnProblemPlay";
            this.btnProblemPlay.Size = new System.Drawing.Size(52, 23);
            this.btnProblemPlay.TabIndex = 5;
            this.btnProblemPlay.Text = "▶";
            this.btnProblemPlay.UseVisualStyleBackColor = true;
            this.btnProblemPlay.Click += new System.EventHandler(this.btnProblemPlay_Click);
            // 
            // btnConfirmPlay
            // 
            this.btnConfirmPlay.Location = new System.Drawing.Point(495, 12);
            this.btnConfirmPlay.Name = "btnConfirmPlay";
            this.btnConfirmPlay.Size = new System.Drawing.Size(52, 23);
            this.btnConfirmPlay.TabIndex = 2;
            this.btnConfirmPlay.Text = "▶";
            this.btnConfirmPlay.UseVisualStyleBackColor = true;
            this.btnConfirmPlay.Click += new System.EventHandler(this.btnConfirmPlay_Click);
            // 
            // btnProblemSelect
            // 
            this.btnProblemSelect.Location = new System.Drawing.Point(12, 41);
            this.btnProblemSelect.Name = "btnProblemSelect";
            this.btnProblemSelect.Size = new System.Drawing.Size(87, 23);
            this.btnProblemSelect.TabIndex = 3;
            this.btnProblemSelect.Text = "problem ♫";
            this.btnProblemSelect.UseVisualStyleBackColor = true;
            this.btnProblemSelect.Click += new System.EventHandler(this.btnProblemSelect_Click);
            // 
            // txtProblemSound
            // 
            this.txtProblemSound.Location = new System.Drawing.Point(105, 41);
            this.txtProblemSound.Name = "txtProblemSound";
            this.txtProblemSound.Size = new System.Drawing.Size(384, 22);
            this.txtProblemSound.TabIndex = 4;
            this.txtProblemSound.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_DragDrop);
            this.txtProblemSound.DragEnter += new System.Windows.Forms.DragEventHandler(this.txt_DragEnter);
            // 
            // btnConfirmSelect
            // 
            this.btnConfirmSelect.Location = new System.Drawing.Point(12, 12);
            this.btnConfirmSelect.Name = "btnConfirmSelect";
            this.btnConfirmSelect.Size = new System.Drawing.Size(87, 23);
            this.btnConfirmSelect.TabIndex = 0;
            this.btnConfirmSelect.Text = "confirm ♫";
            this.btnConfirmSelect.UseVisualStyleBackColor = true;
            this.btnConfirmSelect.Click += new System.EventHandler(this.btnConfirmSelect_Click);
            // 
            // txtConfirmSound
            // 
            this.txtConfirmSound.AllowDrop = true;
            this.txtConfirmSound.Location = new System.Drawing.Point(105, 12);
            this.txtConfirmSound.Name = "txtConfirmSound";
            this.txtConfirmSound.Size = new System.Drawing.Size(384, 22);
            this.txtConfirmSound.TabIndex = 1;
            this.txtConfirmSound.DragDrop += new System.Windows.Forms.DragEventHandler(this.txt_DragDrop);
            this.txtConfirmSound.DragEnter += new System.Windows.Forms.DragEventHandler(this.txt_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "crates";
            // 
            // numCrates
            // 
            this.numCrates.Enabled = false;
            this.numCrates.Location = new System.Drawing.Point(105, 83);
            this.numCrates.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numCrates.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCrates.Name = "numCrates";
            this.numCrates.Size = new System.Drawing.Size(66, 22);
            this.numCrates.TabIndex = 7;
            this.numCrates.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chkCtrl
            // 
            this.chkCtrl.AutoSize = true;
            this.chkCtrl.Checked = true;
            this.chkCtrl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCtrl.Enabled = false;
            this.chkCtrl.Location = new System.Drawing.Point(105, 111);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(66, 21);
            this.chkCtrl.TabIndex = 9;
            this.chkCtrl.Text = "CTRL";
            this.chkCtrl.UseVisualStyleBackColor = true;
            // 
            // chkAlt
            // 
            this.chkAlt.AutoSize = true;
            this.chkAlt.Enabled = false;
            this.chkAlt.Location = new System.Drawing.Point(177, 111);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(56, 21);
            this.chkAlt.TabIndex = 10;
            this.chkAlt.Text = "ALT";
            this.chkAlt.UseVisualStyleBackColor = true;
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Checked = true;
            this.chkShift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShift.Enabled = false;
            this.chkShift.Location = new System.Drawing.Point(249, 111);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(69, 21);
            this.chkShift.TabIndex = 11;
            this.chkShift.Text = "SHIFT";
            this.chkShift.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "mod keys";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(471, 133);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 20);
            this.toolStripStatusLabel1.Text = "label";
            // 
            // AppPrefs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 193);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkAlt);
            this.Controls.Add(this.chkCtrl);
            this.Controls.Add(this.numCrates);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProblemPlay);
            this.Controls.Add(this.btnConfirmPlay);
            this.Controls.Add(this.btnProblemSelect);
            this.Controls.Add(this.txtProblemSound);
            this.Controls.Add(this.btnConfirmSelect);
            this.Controls.Add(this.txtConfirmSound);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppPrefs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "preferences";
            this.Load += new System.EventHandler(this.AppPrefs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numCrates)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProblemPlay;
        private System.Windows.Forms.Button btnConfirmPlay;
        private System.Windows.Forms.Button btnProblemSelect;
        private System.Windows.Forms.TextBox txtProblemSound;
        private System.Windows.Forms.Button btnConfirmSelect;
        private System.Windows.Forms.TextBox txtConfirmSound;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numCrates;
        private System.Windows.Forms.CheckBox chkCtrl;
        private System.Windows.Forms.CheckBox chkAlt;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}