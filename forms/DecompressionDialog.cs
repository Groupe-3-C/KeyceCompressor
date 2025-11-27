using KeyceCompressor.Huffman;
using KeyceCompressor.Zip;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace KeyceCompressor.Forms
{
    public partial class DecompressionDialog : Form
    {
        private readonly string archivePath;
        private BackgroundWorker worker;
        private CancellationTokenSource cts;

        public string Format { get; set; } = ".keyce";

        public DecompressionDialog(string archive)
        {
            archivePath = archive ?? throw new ArgumentNullException(nameof(archive));
            InitializeComponent();

            this.Text = $"Décompression → {Path.GetExtension(archive).ToUpper()}";
            txtArchivePath.Text = archivePath;

            string defaultFolder = Path.Combine(
                Path.GetDirectoryName(archivePath) ?? "",
                Path.GetFileNameWithoutExtension(archivePath)
            );
            txtOutputFolder.Text = defaultFolder;

            btnBrowseFolder.Click += (s, e) =>
            {
                using (var fbd = new FolderBrowserDialog { SelectedPath = txtOutputFolder.Text })
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                        txtOutputFolder.Text = fbd.SelectedPath;
                }
            };

            btnDecompressOK.Click += BtnDecompressOK_Click;
            btnDecompressCancel.Click += BtnDecompressCancel_Click;
        }

        private void BtnDecompressOK_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtOutputFolder.Text))
            {
                try { Directory.CreateDirectory(txtOutputFolder.Text); }
                catch
                {
                    MessageBox.Show("Dossier de destination invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            btnDecompressOK.Enabled = false;
            btnBrowseFolder.Enabled = false;
            btnDecompressCancel.Text = "Annuler";
            lblStatus.Text = "Décompression en cours...";
            progressBar.Value = 0;

            cts = new CancellationTokenSource();
            worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            worker.ProgressChanged += (s, args) =>
            {
                if (args.ProgressPercentage >= 0 && args.ProgressPercentage <= 100)
                    progressBar.Value = args.ProgressPercentage;
            };

            worker.DoWork += (s, args) =>
            {
                var progress = new Progress<int>(p => worker.ReportProgress(p));

                try
                {
                    if (Format == ".keyce")
                        // L'argument cts.Token a été retiré pour correspondre à la signature de la méthode
                        HuffmanDecompressor.Decompress(archivePath, txtOutputFolder.Text, progress);
                    else if (Format == ".zip")
                        ZipCompressor.Decompress(archivePath, txtOutputFolder.Text, progress);
                    else
                        throw new NotSupportedException($"Format {Format} non pris en charge.");
                }
                catch (OperationCanceledException)
                {
                    args.Cancel = true;
                }
                catch (Exception ex)
                {
                    args.Result = ex;
                }
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                progressBar.Value = 100;

                if (args.Cancelled)
                {
                    lblStatus.Text = "Décompression annulée.";
                    MessageBox.Show("Décompression annulée par l'utilisateur.", "Annulé",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                Exception error = args.Error ?? (args.Result as Exception);

                if (error != null)
                {
                    lblStatus.Text = "Échec de la décompression";
                    MessageBox.Show("Erreur :\n" + error.Message, "Erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                lblStatus.Text = "Décompression terminée !";
                MessageBox.Show($"Décompression réussie !\nDossier : {txtOutputFolder.Text}", "Succès",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            };

            worker.RunWorkerAsync();
        }

        private void BtnDecompressCancel_Click(object sender, EventArgs e)
        {
            if (worker != null && worker.IsBusy)
            {
                cts?.Cancel();
                worker.CancelAsync();
                btnDecompressCancel.Enabled = false;
                btnDecompressCancel.Text = "Annulation...";
                lblStatus.Text = "Annulation en cours...";
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
