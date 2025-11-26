
namespace KeyceCompressor.Forms
{
    partial class CompressionDialog
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
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.lblOutputTitle = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnCompressOK = new System.Windows.Forms.Button();
            this.progressCompression = new System.Windows.Forms.ProgressBar();
            this.lblCompressionStatus = new System.Windows.Forms.Label();
            this.btnCompressCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSourceTitle
            // 
            this.lblSourceTitle.AutoSize = true;
            this.lblSourceTitle.Location = new System.Drawing.Point(20, 20);
            this.lblSourceTitle.Name = "lblSourceTitle";
            this.lblSourceTitle.Size = new System.Drawing.Size(92, 13);
            this.lblSourceTitle.TabIndex = 0;
            this.lblSourceTitle.Text = "A COMPRESSER";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(20, 50);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(416, 20);
            this.txtSourcePath.TabIndex = 1;
            this.txtSourcePath.TextChanged += new System.EventHandler(this.txtSourcePath_TextChanged);
            // 
            // lblOutputTitle
            // 
            this.lblOutputTitle.AutoSize = true;
            this.lblOutputTitle.Location = new System.Drawing.Point(20, 100);
            this.lblOutputTitle.Name = "lblOutputTitle";
            this.lblOutputTitle.Size = new System.Drawing.Size(152, 13);
            this.lblOutputTitle.TabIndex = 2;
            this.lblOutputTitle.Text = "EMPLACEMENT DE SORTIE:";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(20, 130);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(400, 20);
            this.txtOutputPath.TabIndex = 3;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(426, 130);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(45, 23);
            this.btnBrowseOutput.TabIndex = 4;
            this.btnBrowseOutput.Text = "...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            // 
            // btnCompressOK
            // 
            this.btnCompressOK.Location = new System.Drawing.Point(328, 255);
            this.btnCompressOK.Name = "btnCompressOK";
            this.btnCompressOK.Size = new System.Drawing.Size(130, 45);
            this.btnCompressOK.TabIndex = 5;
            this.btnCompressOK.Text = "COMPRESSER";
            this.btnCompressOK.UseVisualStyleBackColor = true;
            // 
            // progressCompression
            // 
            this.progressCompression.Location = new System.Drawing.Point(20, 172);
            this.progressCompression.Name = "progressCompression";
            this.progressCompression.Size = new System.Drawing.Size(300, 30);
            this.progressCompression.TabIndex = 6;
            // 
            // lblCompressionStatus
            // 
            this.lblCompressionStatus.AutoSize = true;
            this.lblCompressionStatus.Location = new System.Drawing.Point(20, 240);
            this.lblCompressionStatus.Name = "lblCompressionStatus";
            this.lblCompressionStatus.Size = new System.Drawing.Size(123, 13);
            this.lblCompressionStatus.TabIndex = 7;
            this.lblCompressionStatus.Text = "Prêt pour la compression";
            // 
            // btnCompressCancel
            // 
            this.btnCompressCancel.Location = new System.Drawing.Point(203, 255);
            this.btnCompressCancel.Name = "btnCompressCancel";
            this.btnCompressCancel.Size = new System.Drawing.Size(100, 45);
            this.btnCompressCancel.TabIndex = 8;
            this.btnCompressCancel.Text = "ANNULER";
            this.btnCompressCancel.UseVisualStyleBackColor = true;
            // 
            // CompressionDialog
            // 
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.btnCompressCancel);
            this.Controls.Add(this.lblCompressionStatus);
            this.Controls.Add(this.progressCompression);
            this.Controls.Add(this.btnCompressOK);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblOutputTitle);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.lblSourceTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CompressionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COMPRESSION EN COURS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSourceTitle;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblOutputTitle;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnCompressOK;
        private System.Windows.Forms.ProgressBar progressCompression;
        private System.Windows.Forms.Label lblCompressionStatus;
        private System.Windows.Forms.Button btnCompressCancel;
    }
}