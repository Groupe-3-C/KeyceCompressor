using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using KeyceCompressor.Huffman;

namespace KeyceCompressor.Forms
{
    public partial class DecompressionDialog : Form
    {
        private readonly string keyceFile;
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public DecompressionDialog(string file)
        {
            keyceFile = file ?? "";
            InitializeComponent();

            // Initialiser valeurs contrôles du designer
            lblKeyceSourceTitle.Text = "Fichier .keyce : " + keyceFile;
            var dir = Path.GetDirectoryName(keyceFile);
            if (string.IsNullOrEmpty(dir)) dir = Environment.CurrentDirectory;
            txtDestFolder.Text = Path.Combine(dir, Path.GetFileNameWithoutExtension(keyceFile));
            lblDecompressionStatus.Text = "Statut : Prêt pour la décompression.";

            // btnModify (renommé de btnBrowseDest)
            btnBrowseDest.Click += (s, e) =>
            {
                using (var fbd = new FolderBrowserDialog { SelectedPath = txtDestFolder.Text })
                {
                    if (fbd.ShowDialog() == DialogResult.OK) txtDestFolder.Text = fbd.SelectedPath;
                }
            };

            // btnOK (renommé de btnDecompressOK)
            btnDecompressOK.Click += (s, e) =>
            {
                btnDecompressOK.Enabled = false;
                btnBrowseDest.Enabled = false;
                lblDecompressionStatus.Text = "Décompression en cours...";

                worker.DoWork += (s2, e2) =>
                {
                    // Vérifier si l'annulation a été demandée
                    if (worker.CancellationPending)
                    {
                        e2.Cancel = true;
                        return;
                    }

                    HuffmanDecompressor.Decompress(keyceFile, txtDestFolder.Text, new Progress<int>(p =>
                    {
                        try { progressDecompression.Value = p; } catch { }
                        if (worker.CancellationPending)
                        {
                            // On ne peut pas annuler directement ici, mais on peut signaler
                            // au RunWorkerCompleted que l'annulation a été demandée.
                        }
                    }));
                };

                worker.WorkerSupportsCancellation = true;
                worker.RunWorkerCompleted += (s2, e2) =>
                {
                    btnDecompressOK.Enabled = true;
                    btnBrowseDest.Enabled = true;

                    if (e2.Cancelled)
                    {
                        lblDecompressionStatus.Text = "Annulée !";
                        MessageBox.Show("Décompression annulée !", "Annulation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                    else if (e2.Error != null)
                    {
                        lblDecompressionStatus.Text = "Erreur !";
                        MessageBox.Show("Erreur : " + e2.Error.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = DialogResult.Abort;
                        Close();
                    }
                    else
                    {
                        lblDecompressionStatus.Text = "Succès ! Décompression terminée.";
                        MessageBox.Show("Décompression terminée !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                };
                worker.RunWorkerAsync();
            };

            // btnCancel (renommé de btnDecompressCancel)
            btnDecompressCancel.Click += (s, e) =>
            {
                if (worker.IsBusy)
                {
                    worker.CancelAsync();
                    lblDecompressionStatus.Text = "Annulation demandée...";
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            };
        }
    }
}
