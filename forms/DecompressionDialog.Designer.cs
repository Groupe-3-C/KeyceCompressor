namespace KeyceCompressor.Forms
{
    partial class DecompressionDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblArchive = new System.Windows.Forms.Label();
            this.txtArchivePath = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnDecompressOK = new System.Windows.Forms.Button();
            this.btnDecompressCancel = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblArchive
            // 
            this.lblArchive.AutoSize = true;
            this.lblArchive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblArchive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblArchive.Location = new System.Drawing.Point(20, 70);
            this.lblArchive.Name = "lblArchive";
            this.lblArchive.Size = new System.Drawing.Size(141, 15);
            this.lblArchive.TabIndex = 1;
            this.lblArchive.Text = "Archive à décompresser";
            // 
            // txtArchivePath
            // 
            this.txtArchivePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.txtArchivePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArchivePath.ForeColor = System.Drawing.Color.White;
            this.txtArchivePath.Location = new System.Drawing.Point(20, 95);
            this.txtArchivePath.Name = "txtArchivePath";
            this.txtArchivePath.ReadOnly = true;
            this.txtArchivePath.Size = new System.Drawing.Size(540, 23);
            this.txtArchivePath.TabIndex = 2;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblFolder.Location = new System.Drawing.Point(20, 135);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(130, 15);
            this.lblFolder.TabIndex = 3;
            this.lblFolder.Text = "Dossier de destination";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.txtOutputFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputFolder.ForeColor = System.Drawing.Color.White;
            this.txtOutputFolder.Location = new System.Drawing.Point(20, 160);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(480, 23);
            this.txtOutputFolder.TabIndex = 4;
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnBrowseFolder.FlatAppearance.BorderSize = 0;
            this.btnBrowseFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseFolder.ForeColor = System.Drawing.Color.White;
            this.btnBrowseFolder.Location = new System.Drawing.Point(510, 160);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(50, 25);
            this.btnBrowseFolder.TabIndex = 5;
            this.btnBrowseFolder.Text = "...";
            this.btnBrowseFolder.UseVisualStyleBackColor = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 210);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(540, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 245);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(112, 15);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Prêt à décompresser";
            // 
            // btnDecompressOK
            // 
            this.btnDecompressOK.BackColor = System.Drawing.Color.DimGray;
            this.btnDecompressOK.FlatAppearance.BorderSize = 0;
            this.btnDecompressOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecompressOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDecompressOK.ForeColor = System.Drawing.Color.White;
            this.btnDecompressOK.Location = new System.Drawing.Point(350, 285);
            this.btnDecompressOK.Name = "btnDecompressOK";
            this.btnDecompressOK.Size = new System.Drawing.Size(100, 35);
            this.btnDecompressOK.TabIndex = 8;
            this.btnDecompressOK.Text = "Décompresser";
            this.btnDecompressOK.UseVisualStyleBackColor = false;
            // 
            // btnDecompressCancel
            // 
            this.btnDecompressCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDecompressCancel.FlatAppearance.BorderSize = 0;
            this.btnDecompressCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDecompressCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDecompressCancel.ForeColor = System.Drawing.Color.White;
            this.btnDecompressCancel.Location = new System.Drawing.Point(460, 285);
            this.btnDecompressCancel.Name = "btnDecompressCancel";
            this.btnDecompressCancel.Size = new System.Drawing.Size(100, 35);
            this.btnDecompressCancel.TabIndex = 9;
            this.btnDecompressCancel.Text = "Annuler";
            this.btnDecompressCancel.UseVisualStyleBackColor = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Silver;
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(584, 50);
            this.panelTop.TabIndex = 0;
            // 
            // DecompressionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(584, 340);
            this.Controls.Add(this.btnDecompressCancel);
            this.Controls.Add(this.btnDecompressOK);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnBrowseFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtArchivePath);
            this.Controls.Add(this.lblArchive);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DecompressionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Décompression";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblArchive;
        private System.Windows.Forms.TextBox txtArchivePath;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseFolder;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDecompressOK;
        private System.Windows.Forms.Button btnDecompressCancel;
        private System.Windows.Forms.Panel panelTop;
    }
}