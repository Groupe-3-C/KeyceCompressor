namespace KeyceCompressor.Forms
{
    partial class CompressionDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCompressOK = new System.Windows.Forms.Button();
            this.btnCompressCancel = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupFormat = new System.Windows.Forms.GroupBox();
            this.radioKeyce = new System.Windows.Forms.RadioButton();
            this.radioZip = new System.Windows.Forms.RadioButton();
            this.groupFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblSource.Location = new System.Drawing.Point(20, 75);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(123, 15);
            this.lblSource.TabIndex = 9;
            this.lblSource.Text = "Source à compresser";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.txtSourcePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourcePath.ForeColor = System.Drawing.Color.White;
            this.txtSourcePath.Location = new System.Drawing.Point(20, 100);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(580, 23);
            this.txtSourcePath.TabIndex = 8;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblOutput.Location = new System.Drawing.Point(20, 230);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(96, 15);
            this.lblOutput.TabIndex = 6;
            this.lblOutput.Text = "Enregistrer sous";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.txtOutputPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputPath.ForeColor = System.Drawing.Color.White;
            this.txtOutputPath.Location = new System.Drawing.Point(20, 255);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(510, 23);
            this.txtOutputPath.TabIndex = 5;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnBrowseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutput.ForeColor = System.Drawing.Color.White;
            this.btnBrowseOutput.Location = new System.Drawing.Point(540, 255);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(60, 28);
            this.btnBrowseOutput.TabIndex = 4;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.UseVisualStyleBackColor = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 310);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(580, 25);
            this.progressBar.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 345);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(99, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Prêt à compresser";
            // 
            // btnCompressOK
            // 
            this.btnCompressOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnCompressOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompressOK.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCompressOK.ForeColor = System.Drawing.Color.White;
            this.btnCompressOK.Location = new System.Drawing.Point(400, 380);
            this.btnCompressOK.Name = "btnCompressOK";
            this.btnCompressOK.Size = new System.Drawing.Size(100, 40);
            this.btnCompressOK.TabIndex = 1;
            this.btnCompressOK.Text = "Compresser";
            this.btnCompressOK.UseVisualStyleBackColor = false;
            // 
            // btnCompressCancel
            // 
            this.btnCompressCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnCompressCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompressCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCompressCancel.ForeColor = System.Drawing.Color.White;
            this.btnCompressCancel.Location = new System.Drawing.Point(510, 380);
            this.btnCompressCancel.Name = "btnCompressCancel";
            this.btnCompressCancel.Size = new System.Drawing.Size(90, 40);
            this.btnCompressCancel.TabIndex = 0;
            this.btnCompressCancel.Text = "Annuler";
            this.btnCompressCancel.UseVisualStyleBackColor = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(64)))));
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(620, 60);
            this.panelTop.TabIndex = 0;
            // 
            // groupFormat
            // 
            this.groupFormat.Controls.Add(this.radioKeyce);
            this.groupFormat.Controls.Add(this.radioZip);
            this.groupFormat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.groupFormat.Location = new System.Drawing.Point(20, 140);
            this.groupFormat.Name = "groupFormat";
            this.groupFormat.Size = new System.Drawing.Size(580, 87);
            this.groupFormat.TabIndex = 7;
            this.groupFormat.TabStop = false;
            this.groupFormat.Text = "Format de compression";
            // 
            // radioKeyce
            // 
            this.radioKeyce.AutoSize = true;
            this.radioKeyce.Checked = true;
            this.radioKeyce.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radioKeyce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.radioKeyce.Location = new System.Drawing.Point(20, 30);
            this.radioKeyce.Name = "radioKeyce";
            this.radioKeyce.Size = new System.Drawing.Size(300, 23);
            this.radioKeyce.TabIndex = 0;
            this.radioKeyce.TabStop = true;
            this.radioKeyce.Text = ".keyce → Meilleur ratio (algorithme Huffman)";
            // 
            // radioZip
            // 
            this.radioZip.AutoSize = true;
            this.radioZip.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radioZip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.radioZip.Location = new System.Drawing.Point(20, 55);
            this.radioZip.Name = "radioZip";
            this.radioZip.Size = new System.Drawing.Size(241, 23);
            this.radioZip.TabIndex = 1;
            this.radioZip.Text = ".zip → Standard, rapide et universel";
            // 
            // CompressionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(620, 440);
            this.Controls.Add(this.btnCompressCancel);
            this.Controls.Add(this.btnCompressOK);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.groupFormat);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompressionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compression";
            this.groupFormat.ResumeLayout(false);
            this.groupFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCompressOK;
        private System.Windows.Forms.Button btnCompressCancel;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox groupFormat;
        private System.Windows.Forms.RadioButton radioKeyce;
        private System.Windows.Forms.RadioButton radioZip;
    }
}