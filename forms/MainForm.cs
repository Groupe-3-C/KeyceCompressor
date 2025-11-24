using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

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

                    using (var dlg = new CompressionDialog(sourcePath))
                    {
                        var res = dlg.ShowDialog(this);
                        if (res == DialogResult.OK)
                        {
                            var outFull = dlg.OutputPath;
                            var outName = !string.IsNullOrEmpty(outFull) ? Path.GetFileName(outFull) : Path.GetFileName(sourcePath);
                            AddToHistory(outName, outFull);
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

            using (var dlg = new CompressionDialog(selectedPath))
            {
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    var outFull = dlg.OutputPath;
                    var outName = !string.IsNullOrEmpty(outFull) ? Path.GetFileName(outFull) : Path.GetFileName(selectedPath);
                    AddToHistory(outName, outFull);
                    lblStatus.Text = $"Compression terminée : {outName}";
                }
                else if (res == DialogResult.Cancel)
                {
                    lblStatus.Text = "Compression annulée";
                }
            }
        }

        // Ajoute une entrée cliquable dans l'historique. fullPath peut être null si inconnu.
        private void AddToHistory(string displayName, string fullPath = null)
        {
            var item = new Label
            {
                Text = displayName,
                AutoSize = false,
                Width = flpHistory.ClientSize.Width - 4,
                Height = 24,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(4),
                Margin = new Padding(2),
                Cursor = Cursors.Hand,
                Tag = fullPath
            };

            item.Click += (s, e) =>
            {
                var tag = (s as Label)?.Tag as string;
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

            flpHistory.Controls.Add(item);
        }
    }
}