using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using KeyceCompressor.Huffman;
using KeyceCompressor.Zip;

namespace KeyceCompressor.Forms
{
    public partial class CompressionDialog : Form
    {
        private readonly string sourcePath;
        private BackgroundWorker worker;

        public string Format { get; private set; } = ".keyce";
        public string OutputPath => txtOutputPath.Text;

        public CompressionDialog(string source)
        {
            sourcePath = source ?? throw new ArgumentNullException(nameof(source));
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Compression - Choisissez le format";
            txtSourcePath.Text = sourcePath;
            UpdateOutputPath();

            radioKeyce.CheckedChanged += (s, e) => { if (radioKeyce.Checked) { Format = ".keyce"; UpdateOutputPath(); } };
            radioZip.CheckedChanged += (s, e) => { if (radioZip.Checked) { Format = ".zip"; UpdateOutputPath(); } };

            btnBrowseOutput.Click += BtnBrowseOutput_Click;
            btnCompressOK.Click += BtnCompressOK_Click;
            btnCompressCancel.Click += BtnCompressCancel_Click;
        }

        private void UpdateOutputPath()
        {
            string directory = Path.GetDirectoryName(sourcePath);
            string fileName = Path.GetFileNameWithoutExtension(sourcePath);

            if (Directory.Exists(sourcePath))
            {
                string parentName = new DirectoryInfo(sourcePath).Name;
                fileName = string.IsNullOrEmpty(parentName) ? "Archive" : parentName;
            }

            txtOutputPath.Text = Path.Combine(directory ?? "", fileName + Format);
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = Path.GetFileName(txtOutputPath.Text),
                Filter = radioKeyce.Checked
                    ? "Fichier Keyce (*.keyce)|*.keyce|Tous les fichiers (*.*)|*.*"
                    : "Archive ZIP (*.zip)|*.zip|Tous les fichiers (*.*)|*.*",
                Title = "Enregistrer la compression"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                txtOutputPath.Text = sfd.FileName;
        }

        private void BtnCompressOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOutputPath.Text) || Directory.Exists(txtOutputPath.Text))
            {
                MessageBox.Show("Veuillez spécifier un nom de fichier valide.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnCompressOK.Enabled = false;
            btnBrowseOutput.Enabled = false;
            radioKeyce.Enabled = false;
            radioZip.Enabled = false;
            btnCompressCancel.Text = "Annuler";
            lblStatus.Text = "Compression en cours...";
            progressBar.Value = 0;

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
                        HuffmanCompressor.Compress(sourcePath, txtOutputPath.Text, progress);
                    else
                        ZipCompressor.Compress(sourcePath, txtOutputPath.Text, progress);
                }
                catch (Exception ex)
                {
                    args.Result = ex; // On ne touche PAS à args.Cancel
                }
            };

            worker.RunWorkerCompleted += (s, args) =>
            {
                progressBar.Value = 100;

                if (args.Cancelled)
                {
                    lblStatus.Text = "Compression annulée.";
                    MessageBox.Show("Compression annulée par l'utilisateur.", "Annulé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                // CORRECTION : on récupère l'exception proprement, sans erreur de compilation
                Exception error = args.Error ?? (args.Result as Exception);

                if (error != null)
                {
                    lblStatus.Text = "Échec de la compression.";
                    MessageBox.Show($"Erreur lors de la compression :\n\n{error.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                lblStatus.Text = "Compression terminée avec succès !";
                MessageBox.Show($"Compression réussie !\nFichier sauvegardé :\n{txtOutputPath.Text}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            };

            worker.RunWorkerAsync();
        }

        private void BtnCompressCancel_Click(object sender, EventArgs e)
        {
            if (worker != null && worker.IsBusy)
            {
                worker.CancelAsync();
                btnCompressCancel.Enabled = false;
                btnCompressCancel.Text = "Annulation...";
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