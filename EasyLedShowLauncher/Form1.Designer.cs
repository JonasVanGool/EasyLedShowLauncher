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
            this.SuspendLayout();
            // 
            // cmbComports
            // 
            this.cmbComports.BackColor = System.Drawing.Color.Gray;
            this.cmbComports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbComports.FormattingEnabled = true;
            this.cmbComports.Location = new System.Drawing.Point(92, 12);
            this.cmbComports.Name = "cmbComports";
            this.cmbComports.Size = new System.Drawing.Size(76, 21);
            this.cmbComports.TabIndex = 0;
            // 
            // cmbMidiDevices
            // 
            this.cmbMidiDevices.BackColor = System.Drawing.Color.Gray;
            this.cmbMidiDevices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbMidiDevices.FormattingEnabled = true;
            this.cmbMidiDevices.Location = new System.Drawing.Point(218, 12);
            this.cmbMidiDevices.Name = "cmbMidiDevices";
            this.cmbMidiDevices.Size = new System.Drawing.Size(121, 21);
            this.cmbMidiDevices.TabIndex = 1;
            // 
            // lblComport
            // 
            this.lblComport.AutoSize = true;
            this.lblComport.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.lblComport.Location = new System.Drawing.Point(0, 15);
            this.lblComport.Name = "lblComport";
            this.lblComport.Size = new System.Drawing.Size(86, 13);
            this.lblComport.TabIndex = 2;
            this.lblComport.Text = "Raw DMX COM:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Location = new System.Drawing.Point(183, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Midi:";
            // 
            // txtLaunchDelay
            // 
            this.txtLaunchDelay.BackColor = System.Drawing.Color.Gray;
            this.txtLaunchDelay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtLaunchDelay.Location = new System.Drawing.Point(434, 12);
            this.txtLaunchDelay.Name = "txtLaunchDelay";
            this.txtLaunchDelay.Size = new System.Drawing.Size(33, 20);
            this.txtLaunchDelay.TabIndex = 4;
            this.txtLaunchDelay.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label2.Location = new System.Drawing.Point(473, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sec.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Location = new System.Drawing.Point(354, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Launch delay:";
            // 
            // txtJinxFilePath
            // 
            this.txtJinxFilePath.BackColor = System.Drawing.Color.Gray;
            this.txtJinxFilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtJinxFilePath.Location = new System.Drawing.Point(92, 43);
            this.txtJinxFilePath.Name = "txtJinxFilePath";
            this.txtJinxFilePath.Size = new System.Drawing.Size(247, 20);
            this.txtJinxFilePath.TabIndex = 7;
            this.txtJinxFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label4.Location = new System.Drawing.Point(34, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "JINX! file:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(357, 41);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(134, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 105);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(127, 105);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(109, 23);
            this.btnLaunch.TabIndex = 11;
            this.btnLaunch.Text = "Launch";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label5.Location = new System.Drawing.Point(9, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "JINX! program:";
            // 
            // txtJinxProgram
            // 
            this.txtJinxProgram.BackColor = System.Drawing.Color.Gray;
            this.txtJinxProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtJinxProgram.Location = new System.Drawing.Point(92, 72);
            this.txtJinxProgram.Name = "txtJinxProgram";
            this.txtJinxProgram.Size = new System.Drawing.Size(247, 20);
            this.txtJinxProgram.TabIndex = 13;
            this.txtJinxProgram.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBrowseProgram
            // 
            this.btnBrowseProgram.Location = new System.Drawing.Point(357, 69);
            this.btnBrowseProgram.Name = "btnBrowseProgram";
            this.btnBrowseProgram.Size = new System.Drawing.Size(134, 23);
            this.btnBrowseProgram.TabIndex = 14;
            this.btnBrowseProgram.Text = "Browse";
            this.btnBrowseProgram.UseVisualStyleBackColor = true;
            this.btnBrowseProgram.Click += new System.EventHandler(this.btnBrowseProgram_Click);
            // 
            // lblReceivedBlock
            // 
            this.lblReceivedBlock.AutoSize = true;
            this.lblReceivedBlock.Location = new System.Drawing.Point(434, 110);
            this.lblReceivedBlock.Name = "lblReceivedBlock";
            this.lblReceivedBlock.Size = new System.Drawing.Size(55, 13);
            this.lblReceivedBlock.TabIndex = 15;
            this.lblReceivedBlock.Text = "                ";
            // 
            // pgbLaunch
            // 
            this.pgbLaunch.Location = new System.Drawing.Point(242, 105);
            this.pgbLaunch.Name = "pgbLaunch";
            this.pgbLaunch.Size = new System.Drawing.Size(97, 23);
            this.pgbLaunch.Step = 1;
            this.pgbLaunch.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(357, 105);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label6.Location = new System.Drawing.Point(183, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Jonas Van Gool 2016 V 1.6";
            // 
            // EasyLedShowLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(503, 156);
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
    }
}

