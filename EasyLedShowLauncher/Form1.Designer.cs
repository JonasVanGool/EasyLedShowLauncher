namespace EasyLedShowLauncher
{
    partial class EasyLedShowLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyLedShowLauncher));
            this.cmbComports = new System.Windows.Forms.ComboBox();
            this.cmbMidiDevices = new System.Windows.Forms.ComboBox();
            this.lblComport = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLaunchDelay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtJinxFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtJinxProgram = new System.Windows.Forms.TextBox();
            this.btnBrowseProgram = new System.Windows.Forms.Button();
            this.lblReceivedBlock = new System.Windows.Forms.Label();
            this.pgbLaunch = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMasterReduction = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbComports
            // 
            this.cmbComports.BackColor = System.Drawing.Color.Gray;
            this.cmbComports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbComports.FormattingEnabled = true;
            this.cmbComports.Location = new System.Drawing.Point(123, 15);
            this.cmbComports.Margin = new System.Windows.Forms.Padding(4);
            this.cmbComports.Name = "cmbComports";
            this.cmbComports.Size = new System.Drawing.Size(100, 24);
            this.cmbComports.TabIndex = 0;
            // 
            // cmbMidiDevices
            // 
            this.cmbMidiDevices.BackColor = System.Drawing.Color.Gray;
            this.cmbMidiDevices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbMidiDevices.FormattingEnabled = true;
            this.cmbMidiDevices.Location = new System.Drawing.Point(291, 15);
            this.cmbMidiDevices.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMidiDevices.Name = "cmbMidiDevices";
            this.cmbMidiDevices.Size = new System.Drawing.Size(160, 24);
            this.cmbMidiDevices.TabIndex = 1;
            // 
            // lblComport
            // 
            this.lblComport.AutoSize = true;
            this.lblComport.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.lblComport.Location = new System.Drawing.Point(3, 18);
            this.lblComport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComport.Name = "lblComport";
            this.lblComport.Size = new System.Drawing.Size(108, 17);
            this.lblComport.TabIndex = 2;
            this.lblComport.Text = "Raw DMX COM:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Location = new System.Drawing.Point(244, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Midi:";
            // 
            // txtLaunchDelay
            // 
            this.txtLaunchDelay.BackColor = System.Drawing.Color.Gray;
            this.txtLaunchDelay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtLaunchDelay.Location = new System.Drawing.Point(579, 15);
            this.txtLaunchDelay.Margin = new System.Windows.Forms.Padding(4);
            this.txtLaunchDelay.Name = "txtLaunchDelay";
            this.txtLaunchDelay.Size = new System.Drawing.Size(43, 22);
            this.txtLaunchDelay.TabIndex = 4;
            this.txtLaunchDelay.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.Location = new System.Drawing.Point(631, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sec.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Location = new System.Drawing.Point(472, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Launch delay:";
            // 
            // txtJinxFilePath
            // 
            this.txtJinxFilePath.BackColor = System.Drawing.Color.Gray;
            this.txtJinxFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtJinxFilePath.Location = new System.Drawing.Point(123, 53);
            this.txtJinxFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtJinxFilePath.Name = "txtJinxFilePath";
            this.txtJinxFilePath.Size = new System.Drawing.Size(328, 22);
            this.txtJinxFilePath.TabIndex = 7;
            this.txtJinxFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label4.Location = new System.Drawing.Point(45, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "JINX! file:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(476, 50);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(179, 28);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(13, 168);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 28);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(166, 168);
            this.btnLaunch.Margin = new System.Windows.Forms.Padding(4);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(145, 28);
            this.btnLaunch.TabIndex = 11;
            this.btnLaunch.Text = "Launch";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.Location = new System.Drawing.Point(12, 92);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "JINX! program:";
            // 
            // txtJinxProgram
            // 
            this.txtJinxProgram.BackColor = System.Drawing.Color.Gray;
            this.txtJinxProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtJinxProgram.Location = new System.Drawing.Point(123, 89);
            this.txtJinxProgram.Margin = new System.Windows.Forms.Padding(4);
            this.txtJinxProgram.Name = "txtJinxProgram";
            this.txtJinxProgram.Size = new System.Drawing.Size(328, 22);
            this.txtJinxProgram.TabIndex = 13;
            this.txtJinxProgram.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBrowseProgram
            // 
            this.btnBrowseProgram.Location = new System.Drawing.Point(476, 85);
            this.btnBrowseProgram.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseProgram.Name = "btnBrowseProgram";
            this.btnBrowseProgram.Size = new System.Drawing.Size(179, 28);
            this.btnBrowseProgram.TabIndex = 14;
            this.btnBrowseProgram.Text = "Browse";
            this.btnBrowseProgram.UseVisualStyleBackColor = true;
            this.btnBrowseProgram.Click += new System.EventHandler(this.btnBrowseProgram_Click);
            // 
            // lblReceivedBlock
            // 
            this.lblReceivedBlock.AutoSize = true;
            this.lblReceivedBlock.Location = new System.Drawing.Point(576, 174);
            this.lblReceivedBlock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReceivedBlock.Name = "lblReceivedBlock";
            this.lblReceivedBlock.Size = new System.Drawing.Size(72, 17);
            this.lblReceivedBlock.TabIndex = 15;
            this.lblReceivedBlock.Text = "                ";
            // 
            // pgbLaunch
            // 
            this.pgbLaunch.Location = new System.Drawing.Point(320, 168);
            this.pgbLaunch.Margin = new System.Windows.Forms.Padding(4);
            this.pgbLaunch.Name = "pgbLaunch";
            this.pgbLaunch.Size = new System.Drawing.Size(129, 28);
            this.pgbLaunch.Step = 1;
            this.pgbLaunch.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(473, 168);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 28);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label6.Location = new System.Drawing.Point(241, 204);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "Jonas Van Gool 2016 V 2.1";
            // 
            // txtMasterReduction
            // 
            this.txtMasterReduction.BackColor = System.Drawing.Color.Gray;
            this.txtMasterReduction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtMasterReduction.Location = new System.Drawing.Point(123, 125);
            this.txtMasterReduction.Margin = new System.Windows.Forms.Padding(4);
            this.txtMasterReduction.Name = "txtMasterReduction";
            this.txtMasterReduction.Size = new System.Drawing.Size(100, 22);
            this.txtMasterReduction.TabIndex = 20;
            this.txtMasterReduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label7.Location = new System.Drawing.Point(13, 128);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "Master Reduc:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label8.Location = new System.Drawing.Point(231, 128);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 17);
            this.label8.TabIndex = 21;
            this.label8.Text = "%";
            // 
            // EasyLedShowLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(676, 228);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtMasterReduction);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pgbLaunch);
            this.Controls.Add(this.lblReceivedBlock);
            this.Controls.Add(this.btnBrowseProgram);
            this.Controls.Add(this.txtJinxProgram);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtJinxFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLaunchDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblComport);
            this.Controls.Add(this.cmbMidiDevices);
            this.Controls.Add(this.cmbComports);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EasyLedShowLauncher";
            this.Text = "EasyLedShow Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbComports;
        private System.Windows.Forms.ComboBox cmbMidiDevices;
        private System.Windows.Forms.Label lblComport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLaunchDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtJinxFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtJinxProgram;
        private System.Windows.Forms.Button btnBrowseProgram;
        private System.Windows.Forms.Label lblReceivedBlock;
        private System.Windows.Forms.ProgressBar pgbLaunch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMasterReduction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

