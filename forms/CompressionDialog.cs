using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System;
using System.Drawing;
using KeyceCompressor.Huffman;

namespace KeyceCompressor.Forms
{
    public partial class CompressionDialog : Form
    {
        private readonly string source;
        private BackgroundWorker worker;

        public CompressionDialog(string src)
        {
            source = src ?? "";
            InitializeComponent();

            // Initialiser valeurs contrôles du designer
            lblSourceTitle.Text = "Source : " + source;
            txtOutputPath.Text = Path.ChangeExtension(source, ".keyce");
            lblCompressionStatus.Text = "Statut : Prêt pour la compression.";
            progressCompression.Value = 0;

            btnBrowseOutput.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog { Filter = "Keyce|*.keyce", FileName = Path.GetFileName(txtOutputPath.Text) })
                {
                    if (sfd.ShowDialog() == DialogResult.OK) txtOutputPath.Text = sfd.FileName;
                }
            };

            btnCompressOK.Click += BtnCompressOK_Click;
            btnCompressCancel.Click += BtnCompressCancel_Click;
        }

        // Expose le chemin de sortie après réussite
        public string OutputPath => txtOutputPath?.Text;

        private void BtnCompressOK_Click(object sender, EventArgs e)
        {
            btnCompressOK.Enabled = false;
            btnBrowseOutput.Enabled = false;
            lblCompressionStatus.Text = "Compression en cours...";
            progressCompression.Value = 0;

            // Nouveau worker pour chaque opération (évite l'accumulation d'handlers)
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += (s2, e2) =>
            {
                // Appeler la méthode synchrone qui effectue la compression
                try
                {
                    HuffmanCompressor.Compress(source, txtOutputPath.Text, new Progress<int>(p =>
                    {
                        try { progressCompression.Value = p; } catch { }
                    }));
                }
                catch (Exception ex)
                {
                    e2.Result = ex;
                    throw;
                }
            };

            worker.RunWorkerCompleted += (s2, e2) =>
            {
                btnCompressOK.Enabled = true;
                btnBrowseOutput.Enabled = true;

                if (e2.Cancelled)
                {
                    lblCompressionStatus.Text = "Annulée !";
                    MessageBox.Show("Compression annulée !", "Annulation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                else if (e2.Error != null)
                {
                    lblCompressionStatus.Text = "Erreur !";
                    MessageBox.Show("Erreur : " + e2.Error.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                    Close();
                }
                else
                {
                    lblCompressionStatus.Text = "Succès ! Compression terminée.";
                    MessageBox.Show("Compression terminée !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };

            worker.RunWorkerAsync();
        }

        private void BtnCompressCancel_Click(object sender, EventArgs e)
        {
            if (worker != null && worker.IsBusy)
            {
                worker.CancelAsync();
                lblCompressionStatus.Text = "Annulation demandée...";
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}