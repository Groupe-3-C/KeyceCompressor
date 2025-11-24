namespace KeyceCompressor.Forms
{
    partial class MainForm
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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.btnCompress = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.tvFiles = new System.Windows.Forms.TreeView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.flpHistory = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressMain = new System.Windows.Forms.ProgressBar();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelLeft.Controls.Add(this.btnCompress);
            this.panelLeft.Controls.Add(this.btnOpen);
            this.panelLeft.Controls.Add(this.tvFiles);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(400, 711);
            this.panelLeft.TabIndex = 0;
            // 
            // btnCompress
            // 
            this.btnCompress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompress.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnCompress.Location = new System.Drawing.Point(20, 580);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(360, 60);
            this.btnCompress.TabIndex = 2;
            this.btnCompress.Text = "COMPRESSER";
            this.btnCompress.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Location = new System.Drawing.Point(20, 520);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(360, 50);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "OUVRIR";
            this.btnOpen.UseVisualStyleBackColor = false;
            // 
            // tvFiles
            // 
            this.tvFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.tvFiles.Location = new System.Drawing.Point(0, 0);
            this.tvFiles.Name = "tvFiles";
            this.tvFiles.Size = new System.Drawing.Size(400, 500);
            this.tvFiles.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panelRight.Controls.Add(this.flpHistory);
            this.panelRight.Controls.Add(this.lblHistoryTitle);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(739, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(445, 711);
            this.panelRight.TabIndex = 4;
            // 
            // flpHistory
            // 
            this.flpHistory.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.flpHistory.Location = new System.Drawing.Point(35, 67);
            this.flpHistory.Name = "flpHistory";
            this.flpHistory.Size = new System.Drawing.Size(384, 268);
            this.flpHistory.TabIndex = 6;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryTitle.Location = new System.Drawing.Point(60, 9);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(335, 28);
            this.lblHistoryTitle.TabIndex = 5;
            this.lblHistoryTitle.Text = "Historique des fichiers .keyce";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelBottom.Controls.Add(this.lblStatus);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(400, 497);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(339, 214);
            this.panelBottom.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.GrayText;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(20, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(133, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Prêt";
            // 
            // progressMain
            // 
            this.progressMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.progressMain.BackColor = System.Drawing.SystemColors.GrayText;
            this.progressMain.Location = new System.Drawing.Point(406, 620);
            this.progressMain.Name = "progressMain";
            this.progressMain.Size = new System.Drawing.Size(314, 36);
            this.progressMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressMain.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1184, 711);
            this.Controls.Add(this.progressMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Name = "MainForm";
            this.Text = "Keyce Compressor v1.0";
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TreeView tvFiles;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.FlowLayoutPanel flpHistory;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressMain;
    }
}