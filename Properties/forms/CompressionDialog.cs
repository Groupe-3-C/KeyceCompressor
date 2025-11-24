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
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public CompressionDialog(string src)
        {
            source = src ?? "";
            InitializeComponent();

            // Initialiser valeurs contrôles du designer
            lblSourceTitle.Text = "Source : " + source;
            txtOutputPath.Text = Path.ChangeExtension(source, ".keyce");
            lblCompressionStatus.Text = "Statut : Prêt pour la compression.";

            // btnModify (renommé de btnBrowseOutput)
            btnBrowseOutput.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog { Filter = "Keyce|*.keyce", FileName = Path.GetFileName(txtOutputPath.Text) })
                {
                    if (sfd.ShowDialog() == DialogResult.OK) txtOutputPath.Text = sfd.FileName;
                }
            };

            // btnOK (renommé de btnCompressOK)
            btnCompressOK.Click += (s, e) =>
            {
                btnCompressOK.Enabled = false;
                btnBrowseOutput.Enabled = false;
                lblCompressionStatus.Text = "Compression en cours...";

                worker.DoWork += (s2, e2) =>
                {
                    // Vérifier si l'annulation a été demandée
                    if (worker.CancellationPending)
                    {
                        e2.Cancel = true;
                        return;
                    }

                    HuffmanCompressor.Compress(source, txtOutputPath.Text, new Progress<int>(p =>
                    {
                        try { progressCompression.Value = p; } catch { }
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
            };

            // btnCancel (renommé de btnCompressCancel)
            btnCompressCancel.Click += (s, e) =>
            {
                if (worker.IsBusy)
                {
                    worker.CancelAsync();
                    lblCompressionStatus.Text = "Annulation demandée...";
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
