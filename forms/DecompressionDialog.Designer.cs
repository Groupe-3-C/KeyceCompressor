namespace KeyceCompressor.Forms
{
    partial class DecompressionDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblSourceTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeycePath = new System.Windows.Forms.TextBox();
            this.lblKeyceSourceTitle = new System.Windows.Forms.Label();
            this.lblDestFolderTitle = new System.Windows.Forms.Label();
            this.txtDestFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseDest = new System.Windows.Forms.Button();
            this.progressDecompression = new System.Windows.Forms.ProgressBar();
            this.lblDecompressionStatus = new System.Windows.Forms.Label();
            this.btnDecompressCancel = new System.Windows.Forms.Button();
            this.btnDecompressOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtKeycePath
            // 
            this.txtKeycePath.Location = new System.Drawing.Point(15, 47);
            this.txtKeycePath.Name = "txtKeycePath";
            this.txtKeycePath.ReadOnly = true;
            this.txtKeycePath.Size = new System.Drawing.Size(416, 20);
            this.txtKeycePath.TabIndex = 3;
            // 
            // lblKeyceSourceTitle
            // 
            this.lblKeyceSourceTitle.AutoSize = true;
            this.lblKeyceSourceTitle.Location = new System.Drawing.Point(18, 9);
            this.lblKeyceSourceTitle.Name = "lblKeyceSourceTitle";
            this.lblKeyceSourceTitle.Size = new System.Drawing.Size(107, 13);
            this.lblKeyceSourceTitle.TabIndex = 4;
            this.lblKeyceSourceTitle.Text = "A DECOMPRESSER";
            // 
            // lblDestFolderTitle
            // 
            this.lblDestFolderTitle.AutoSize = true;
            this.lblDestFolderTitle.Location = new System.Drawing.Point(18, 90);
            this.lblDestFolderTitle.Name = "lblDestFolderTitle";
            this.lblDestFolderTitle.Size = new System.Drawing.Size(152, 13);
            this.lblDestFolderTitle.TabIndex = 5;
            this.lblDestFolderTitle.Text = "EMPLACEMENT DE SORTIE:";
            // 
            // txtDestFolder
            // 
            this.txtDestFolder.Location = new System.Drawing.Point(12, 120);
            this.txtDestFolder.Name = "txtDestFolder";
            this.txtDestFolder.Size = new System.Drawing.Size(400, 20);
            this.txtDestFolder.TabIndex = 6;
            // 
            // btnBrowseDest
            // 
            this.btnBrowseDest.Location = new System.Drawing.Point(465, 120);
            this.btnBrowseDest.Name = "btnBrowseDest";
            this.btnBrowseDest.Size = new System.Drawing.Size(45, 23);
            this.btnBrowseDest.TabIndex = 7;
            this.btnBrowseDest.Text = "...";
            this.btnBrowseDest.UseVisualStyleBackColor = true;
            // 
            // progressDecompression
            // 
            this.progressDecompression.Location = new System.Drawing.Point(12, 167);
            this.progressDecompression.Name = "progressDecompression";
            this.progressDecompression.Size = new System.Drawing.Size(300, 30);
            this.progressDecompression.TabIndex = 8;
            // 
            // lblDecompressionStatus
            // 
            this.lblDecompressionStatus.AutoSize = true;
            this.lblDecompressionStatus.Location = new System.Drawing.Point(9, 232);
            this.lblDecompressionStatus.Name = "lblDecompressionStatus";
            this.lblDecompressionStatus.Size = new System.Drawing.Size(123, 13);
            this.lblDecompressionStatus.TabIndex = 9;
            this.lblDecompressionStatus.Text = "Prêt pour la décompression";
            // 
            // btnDecompressCancel
            // 
            this.btnDecompressCancel.Location = new System.Drawing.Point(212, 262);
            this.btnDecompressCancel.Name = "btnDecompressCancel";
            this.btnDecompressCancel.Size = new System.Drawing.Size(100, 45);
            this.btnDecompressCancel.TabIndex = 10;
            this.btnDecompressCancel.Text = "ANNULER";
            this.btnDecompressCancel.UseVisualStyleBackColor = true;
            // 
            // btnDecompressOK
            // 
            this.btnDecompressOK.Location = new System.Drawing.Point(380, 262);
            this.btnDecompressOK.Name = "btnDecompressOK";
            this.btnDecompressOK.Size = new System.Drawing.Size(130, 45);
            this.btnDecompressOK.TabIndex = 11;
            this.btnDecompressOK.Text = "DECOMPRESSER";
            this.btnDecompressOK.UseVisualStyleBackColor = true;
            // 
            // DecompressionDialog
            // 
            this.ClientSize = new System.Drawing.Size(630, 319);
            this.Controls.Add(this.btnDecompressOK);
            this.Controls.Add(this.btnDecompressCancel);
            this.Controls.Add(this.lblDecompressionStatus);
            this.Controls.Add(this.progressDecompression);
            this.Controls.Add(this.btnBrowseDest);
            this.Controls.Add(this.txtDestFolder);
            this.Controls.Add(this.lblDestFolderTitle);
            this.Controls.Add(this.lblKeyceSourceTitle);
            this.Controls.Add(this.txtKeycePath);
            this.Name = "DecompressionDialog";
            this.Text = "Décompression .keyce";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblSourceTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeycePath;
        private System.Windows.Forms.Label lblKeyceSourceTitle;
        private System.Windows.Forms.Label lblDestFolderTitle;
        private System.Windows.Forms.TextBox txtDestFolder;
        private System.Windows.Forms.Button btnBrowseDest;
        private System.Windows.Forms.ProgressBar progressDecompression;
        private System.Windows.Forms.Label lblDecompressionStatus;
        private System.Windows.Forms.Button btnDecompressCancel;
        private System.Windows.Forms.Button btnDecompressOK;
    }
}