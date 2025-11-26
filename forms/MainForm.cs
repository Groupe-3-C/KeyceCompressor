using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using KeyceCompressor.Models;

namespace KeyceCompressor.Forms
{
    public partial class MainForm : Form
    {
        // Contient les éléments choisis via OUVRIR (fichiers et/ou dossier).
        // Si vide, on compresse le nœud sélectionné dans l'arborescence.
        private readonly List<string> selectedSources = new List<string>();

        public MainForm()
        {
            InitializeComponent();

            // Wire up events (les boutons existent dans le Designer)
            btnOpen.Click += BtnOpen_Click;
            btnCompress.Click += BtnCompress_Click;
            tvFiles.NodeMouseDoubleClick += TvFiles_NodeMouseDoubleClick;
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;
            tvFiles.AfterSelect += TvFiles_AfterSelect;

            // Initialise l'arborescence depuis les lecteurs racines
            InitializeTree();

            // Ajout des en-têtes de l'historique
            AddHistoryHeaders();
        }

        // Nouvelle fonction pour calculer la taille d'un dossier de manière récursive
        private static long GetDirectorySize(string path)
        {
            long size = 0;
            try
            {
                // Ajoute la taille des fichiers dans le répertoire courant
                foreach (string file in Directory.GetFiles(path))
                {
                    if (File.Exists(file))
                    {
                        size += new FileInfo(file).Length;
                    }
                }

                // Ajoute la taille des sous-répertoires (récursif)
                foreach (string dir in Directory.GetDirectories(path))
                {
                    size += GetDirectorySize(dir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignorer les dossiers non accessibles
            }
            catch (PathTooLongException)
            {
                // Ignorer les chemins trop longs
            }
            catch (Exception)
            {
                // Ignorer autres erreurs
            }
            return size;
        }

        private void InitializeTree()
        {
            tvFiles.Nodes.Clear();
            try
            {
                foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady))
                {
                    var rootPath = drive.RootDirectory.FullName;
                    var rootNode = CreateDirectoryNode(rootPath, drive.Name.TrimEnd('\\'));
                    tvFiles.Nodes.Add(rootNode);
                }
            }
            catch
            {
                // silencieux si accès refusé aux lecteurs
            }

            lblStatus.Text = "Arborescence chargée";
            btnCompress.Enabled = false;
        }

        private TreeNode CreateDirectoryNode(string fullPath, string displayName = null)
        {
            var node = new TreeNode(displayName ?? Path.GetFileName(fullPath).TrimEnd('\\'))
            {
                Tag = fullPath
            };

            try
            {
                if (Directory.EnumerateFileSystemEntries(fullPath).Any())
                {
                    node.Nodes.Add(new TreeNode("...")); // dummy child
                }
            }
            catch
            {
                // accès refusé -> pas de dummy
            }

            return node;
        }

        private void TvFiles_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;
            var path = node.Tag as string;
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "...")
            {
                node.Nodes.Clear();
                try
                {
                    IEnumerable<string> dirs = Enumerable.Empty<string>();
                    try { dirs = Directory.GetDirectories(path); } catch { }
                    foreach (var d in dirs.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
                    {
                        var name = Path.GetFileName(d);
                        try { node.Nodes.Add(CreateDirectoryNode(d, name)); } catch { }
                    }

                    IEnumerable<string> files = Enumerable.Empty<string>();
                    try { files = Directory.GetFiles(path); } catch { }
                    foreach (var f in files.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
                    {
                        var fn = Path.GetFileName(f);
                        var fnode = new TreeNode(fn) { Tag = f };
                        node.Nodes.Add(fnode);
                    }
                }
                catch
                {
                    // ignore access errors
                }
            }
        }

        private void TvFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var path = e.Node?.Tag as string;
            if (string.IsNullOrEmpty(path))
            {
                lblStatus.Text = "Aucun élément sélectionné";
                btnCompress.Enabled = selectedSources.Count > 0;
                return;
            }

            if (Directory.Exists(path))
            {
                lblStatus.Text = $"Dossier sélectionné : {path}";
                btnCompress.Enabled = true;
            }
            else if (File.Exists(path))
            {
                lblStatus.Text = $"Fichier sélectionné : {path}";
                btnCompress.Enabled = true;
            }
            else
            {
                lblStatus.Text = $"Chemin invalide : {path}";
                btnCompress.Enabled = selectedSources.Count > 0;
            }
        }

        private void TvFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var path = e.Node?.Tag as string;
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{path}\"");
                else if (Directory.Exists(path))
                    System.Diagnostics.Process.Start("explorer.exe", $"\"{path}\"");
                else
                    MessageBox.Show("Le chemin n'existe pas : " + path, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ouvrir l'emplacement : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ajoute la branche depuis la racine et retourne le TreeNode correspondant
        private TreeNode EnsurePathInTree(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return null;

            string rootPath = Path.GetPathRoot(fullPath);
            if (string.IsNullOrEmpty(rootPath)) rootPath = fullPath;

            TreeNode rootNode = tvFiles.Nodes.Cast<TreeNode>()
                .FirstOrDefault(n => string.Equals(n.Tag as string ?? "", rootPath, StringComparison.OrdinalIgnoreCase));

            if (rootNode == null)
            {
                rootNode = CreateDirectoryNode(rootPath, rootPath.TrimEnd('\\'));
                tvFiles.Nodes.Add(rootNode);
            }

            if (string.Equals(rootNode.Tag as string ?? "", fullPath, StringComparison.OrdinalIgnoreCase))
            {
                rootNode.EnsureVisible();
                rootNode.Expand();
                return rootNode;
            }

            var relative = fullPath.Substring(rootPath.Length).Trim(Path.DirectorySeparatorChar);
            if (string.IsNullOrEmpty(relative))
            {
                rootNode.EnsureVisible();
                rootNode.Expand();
                return rootNode;
            }

            var parts = relative.Split(Path.DirectorySeparatorChar);
            TreeNode current = rootNode;
            string accum = rootPath.TrimEnd(Path.DirectorySeparatorChar);

            foreach (var part in parts)
            {
                accum = Path.Combine(accum, part);
                var child = current.Nodes.Cast<TreeNode>()
                    .FirstOrDefault(n => string.Equals(n.Tag as string ?? "", accum, StringComparison.OrdinalIgnoreCase)
                                         || string.Equals(n.Text, part, StringComparison.OrdinalIgnoreCase));

                if (child == null)
                {
                    bool isLast = part == parts.Last();
                    if (isLast && File.Exists(accum))
                        child = new TreeNode(part) { Tag = accum };
                    else
                        child = CreateDirectoryNode(accum, part);

                    current.Nodes.Add(child);
                }

                current = child;
                try { current.Expand(); } catch { }
            }

            current.EnsureVisible();
            return current;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            // Réinitialise la sélection interne à chaque nouvel "OUVRIR"
            selectedSources.Clear();

            var choice = MessageBox.Show(
                "Voulez-vous sélectionner un fichier ?\n" +
                "Réponse Oui = fichier, Non = dossier, Annuler = aucune sélection.",
                "Choix d'ouverture",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (choice == DialogResult.Cancel) return;

            if (choice == DialogResult.Yes)
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Multiselect = true;
                    ofd.Title = "Sélectionner les fichiers à compresser";
                    ofd.Filter = "All files (*.*)|*.*";

                    if (ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        TreeNode lastAdded = null;
                        foreach (var file in ofd.FileNames)
                        {
                            selectedSources.Add(file);
                            var node = EnsurePathInTree(file);
                            lastAdded = node ?? lastAdded;
                        }

                        if (lastAdded != null)
                        {
                            tvFiles.SelectedNode = lastAdded;
                            lastAdded.EnsureVisible();
                        }

                        lblStatus.Text = $"{selectedSources.Count} fichier(s) choisi(s) pour compression";
                        progressMain.Value = 0;
                        btnCompress.Enabled = true;
                    }
                }
            }
            else if (choice == DialogResult.No)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Sélectionnez le dossier à compresser";
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        var folder = fbd.SelectedPath;
                        selectedSources.Add(folder);
                        var node = EnsurePathInTree(folder);
                        if (node != null)
                        {
                            tvFiles.SelectedNode = node;
                            node.EnsureVisible();
                        }

                        lblStatus.Text = $"Dossier choisi pour compression : {folder}";
                        progressMain.Value = 0;
                        btnCompress.Enabled = true;
                    }
                }
            }
        }

        private void BtnCompress_Click(object sender, EventArgs e)
        {
            // Si des éléments ont été choisis via OUVRIR -> compresser ces éléments
            if (selectedSources.Count > 0)
            {
                foreach (var sourcePath in selectedSources.ToList())
                {
                    if (string.IsNullOrEmpty(sourcePath)) continue;

                    // Calcul de la taille avant compression
                    long sourceSize = 0;
                    if (File.Exists(sourcePath))
                    {
                        sourceSize = new FileInfo(sourcePath).Length;
                    }
                    else if (Directory.Exists(sourcePath))
                    {
                        sourceSize = GetDirectorySize(sourcePath);
                    }

                    using (var dlg = new CompressionDialog(sourcePath))
                    {
                        var res = dlg.ShowDialog(this);
                        if (res == DialogResult.OK)
                        {
                            var outFull = dlg.OutputPath;
                            var outName = !string.IsNullOrEmpty(outFull) ? Path.GetFileName(outFull) : Path.GetFileName(sourcePath);

                            // Ajout des informations complètes à l'historique
                            AddToHistory(outName, outFull, sourceSize);
                            lblStatus.Text = $"Compression terminée : {outName}";
                        }
                        else if (res == DialogResult.Cancel)
                        {
                            lblStatus.Text = "Compression annulée";
                        }
                    }
                }

                // Après traitement, vider la sélection "OUVRIR" (comportement modifiable)
                selectedSources.Clear();
                btnCompress.Enabled = false;
                return;
            }

            // Sinon, compresser le nœud sélectionné dans l'arborescence
            var node = tvFiles.SelectedNode;
            if (node == null)
            {
                MessageBox.Show("Sélectionnez d'abord un fichier ou dossier dans l'arborescence.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedPath = node.Tag as string;
            if (string.IsNullOrEmpty(selectedPath))
            {
                MessageBox.Show("Chemin invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calcul de la taille avant compression
            long selectedSize = 0;
            if (File.Exists(selectedPath))
            {
                selectedSize = new FileInfo(selectedPath).Length;
            }
            else if (Directory.Exists(selectedPath))
            {
                selectedSize = GetDirectorySize(selectedPath);
            }

            using (var dlg = new CompressionDialog(selectedPath))
            {
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    var outFull = dlg.OutputPath;
                    var outName = !string.IsNullOrEmpty(outFull) ? Path.GetFileName(outFull) : Path.GetFileName(selectedPath);

                    // Ajout des informations complètes à l'historique
                    AddToHistory(outName, outFull, selectedSize);
                    lblStatus.Text = $"Compression terminée : {outName}";
                }
                else if (res == DialogResult.Cancel)
                {
                    lblStatus.Text = "Compression annulée";
                }
            }
        }

        // Ajoute les en-têtes de colonnes à flpHistory
        private void AddHistoryHeaders()
        {
            // Création d'un Panel pour contenir les Labels d'en-tête
            var headerPanel = new Panel
            {
                Name = "pnlHistoryHeader",
                BackColor = System.Drawing.Color.FromArgb(52, 73, 94),
                ForeColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Width = flpHistory.ClientSize.Width - 2,
                Height = 25,
                Tag = "Header"
            };

            // Fonction utilitaire pour créer un Label d'en-tête
            Label CreateHeaderLabel(string text, int width)
            {
                return new Label
                {
                    Text = text,
                    Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    AutoSize = false,
                    Width = width,
                    Height = 25,
                    Padding = new Padding(2),
                    Margin = new Padding(0),
                    BackColor = System.Drawing.Color.Transparent,
                    ForeColor = System.Drawing.Color.White
                };
            }

            // Définition des largeurs
            int widthName = 120;
            int widthBefore = 80;
            int widthAfter = 80;
            int widthDate = 100;

            int totalWidth = widthName + widthBefore + widthAfter + widthDate;

            if (flpHistory.ClientSize.Width > totalWidth)
            {
                widthName += flpHistory.ClientSize.Width - totalWidth - 4;
            }

            // Création et ajout des Labels d'en-tête au Panel
            headerPanel.Controls.Add(CreateHeaderLabel("Nom", widthName));
            headerPanel.Controls.Add(CreateHeaderLabel("Taille avant", widthBefore));
            headerPanel.Controls.Add(CreateHeaderLabel("Taille après", widthAfter));
            headerPanel.Controls.Add(CreateHeaderLabel("Date", widthDate));

            // Positionnement des Labels dans le Panel
            int currentX = 0;
            foreach (Control control in headerPanel.Controls)
            {
                control.Location = new System.Drawing.Point(currentX, 0);
                currentX += control.Width;
            }

            // Ajout du Panel d'en-tête au FlowLayoutPanel
            flpHistory.Controls.Add(headerPanel);
        }

        // Ajoute une entrée cliquable dans l'historique
        private void AddToHistory(string displayName, string fullPath = null, long sizeBefore = 0)
        {
            // Récupération des infos du fichier compressé
            long sizeAfter = 0;
            DateTime modDate = DateTime.Now;
            if (File.Exists(fullPath))
            {
                var fi = new FileInfo(fullPath);
                sizeAfter = fi.Length;
                modDate = fi.LastWriteTime;
            }

            // Gestionnaire de clic pour la décompression
            EventHandler clickHandler = (s, e) =>
            {
                var tag = (s as Control)?.Tag as string;
                if (string.IsNullOrEmpty(tag) || !File.Exists(tag))
                {
                    MessageBox.Show("Fichier .keyce introuvable : " + (tag ?? "(inconnu)"), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var dlg = new DecompressionDialog(tag))
                {
                    var res = dlg.ShowDialog(this);
                    if (res == DialogResult.OK)
                    {
                        lblStatus.Text = "Décompression terminée";
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        lblStatus.Text = "Décompression annulée";
                    }
                }
            };

            // Création d'un Panel pour l'entrée d'historique
            var itemPanel = new Panel
            {
                Name = "pnlHistoryItem_" + Guid.NewGuid().ToString("N"),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 1),
                Padding = new Padding(0),
                Width = flpHistory.ClientSize.Width - 2,
                Height = 24,
                Cursor = Cursors.Hand,
                Tag = fullPath
            };

            itemPanel.Click += clickHandler;

            // Fonction utilitaire pour formater la taille
            string FormatSize(long bytes)
            {
                if (bytes < 1024) return $"{bytes} B";
                if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
                if (bytes < 1024 * 1024 * 1024) return $"{bytes / (1024.0 * 1024.0):F1} MB";
                return $"{bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
            }

            // Définition des largeurs
            int widthName = 120;
            int widthBefore = 80;
            int widthAfter = 80;
            int widthDate = 100;

            int totalWidth = widthName + widthBefore + widthAfter + widthDate;
            if (flpHistory.ClientSize.Width > totalWidth)
            {
                widthName += flpHistory.ClientSize.Width - totalWidth - 4;
            }

            // Fonction utilitaire pour créer un Label de donnée
            Label CreateDataLabel(string text, int width)
            {
                return new Label
                {
                    Text = text,
                    Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    AutoSize = false,
                    Width = width,
                    Height = 24,
                    Padding = new Padding(2),
                    Margin = new Padding(0),
                    BackColor = System.Drawing.Color.Transparent,
                    Tag = fullPath
                };
            }

            // Création des Labels de données
            var lblName = CreateDataLabel(displayName, widthName);
            var lblSizeBefore = CreateDataLabel(FormatSize(sizeBefore), widthBefore);
            var lblSizeAfter = CreateDataLabel(FormatSize(sizeAfter), widthAfter);
            var lblDate = CreateDataLabel(modDate.ToString("dd/MM/yyyy HH:mm"), widthDate);

            // Ajout des Labels au Panel d'entrée
            itemPanel.Controls.Add(lblName);
            itemPanel.Controls.Add(lblSizeBefore);
            itemPanel.Controls.Add(lblSizeAfter);
            itemPanel.Controls.Add(lblDate);

            // Positionnement des Labels dans le Panel
            int currentX = 0;
            foreach (Control control in itemPanel.Controls)
            {
                control.Location = new System.Drawing.Point(currentX, 0);
                currentX += control.Width;
            }

            // Ajout du Panel d'entrée au FlowLayoutPanel
            flpHistory.Controls.Add(itemPanel);

            // Pour que le clic sur n'importe quel Label déclenche le clic sur le Panel parent
            foreach (Control control in itemPanel.Controls)
            {
                control.Click += clickHandler;
            }
        }
    }
}