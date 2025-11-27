using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KeyceCompressor.Models;
using KeyceCompressor.Huffman;
using KeyceCompressor.Zip;

namespace KeyceCompressor.Forms
{
    public partial class MainForm : Form
    {
        private readonly List<string> selectedSources = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            btnOpen.Click += BtnOpen_Click;
            btnCompress.Click += BtnCompress_Click;
            tvFiles.NodeMouseDoubleClick += TvFiles_NodeMouseDoubleClick;
            tvFiles.BeforeExpand += TvFiles_BeforeExpand;
            tvFiles.AfterSelect += TvFiles_AfterSelect;
            InitializeTree();
            AddHistoryHeaders();
        }

        private static long GetDirectorySize(string path)
        {
            long size = 0;
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (File.Exists(file))
                    {
                        size += new FileInfo(file).Length;
                    }
                }
                foreach (string dir in Directory.GetDirectories(path))
                {
                    size += GetDirectorySize(dir);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (PathTooLongException) { }
            catch (Exception) { }
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
            catch { }
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
                    node.Nodes.Add(new TreeNode("..."));
                }
            }
            catch { }
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
                catch { }
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
            string sourcePath = selectedSources.FirstOrDefault();
            if (string.IsNullOrEmpty(sourcePath))
            {
                var node = tvFiles.SelectedNode;
                if (node == null || node.Tag == null)
                {
                    MessageBox.Show("Veuillez sélectionner un fichier ou un dossier à compresser.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sourcePath = node.Tag as string;
            }

            if (string.IsNullOrEmpty(sourcePath) || (!File.Exists(sourcePath) && !Directory.Exists(sourcePath)))
            {
                MessageBox.Show("Chemin invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long sizeBefore = File.Exists(sourcePath) ? new FileInfo(sourcePath).Length : GetDirectorySize(sourcePath);

            using (var dlg = new CompressionDialog(sourcePath))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string outputFile = dlg.OutputPath;
                    string fileName = Path.GetFileName(outputFile);
                    AddToHistory(fileName, outputFile, sizeBefore);
                    lblStatus.Text = $"Compression terminée → {fileName}";
                }
                else
                {
                    lblStatus.Text = "Compression annulée";
                }
            }

            selectedSources.Clear();
            btnCompress.Enabled = tvFiles.SelectedNode != null;
        }

        private void AddHistoryHeaders()
        {
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

            Label CreateHeaderLabel(string text, int width)
            {
                return new Label
                {
                    Text = text,
                    Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
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

            int widthName = 120;
            int widthBefore = 80;
            int widthAfter = 80;
            int widthDate = 100;
            int totalWidth = widthName + widthBefore + widthAfter + widthDate;
            if (flpHistory.ClientSize.Width > totalWidth)
            {
                widthName += flpHistory.ClientSize.Width - totalWidth - 4;
            }

            headerPanel.Controls.Add(CreateHeaderLabel("Nom", widthName));
            headerPanel.Controls.Add(CreateHeaderLabel("Taille avant", widthBefore));
            headerPanel.Controls.Add(CreateHeaderLabel("Taille après", widthAfter));
            headerPanel.Controls.Add(CreateHeaderLabel("Date", widthDate));

            int currentX = 0;
            foreach (Control control in headerPanel.Controls)
            {
                control.Location = new System.Drawing.Point(currentX, 0);
                currentX += control.Width;
            }

            flpHistory.Controls.Add(headerPanel);
        }

        private void AddToHistory(string displayName, string fullPath = null, long sizeBefore = 0)
        {
            long sizeAfter = 0;
            DateTime modDate = DateTime.Now;
            if (File.Exists(fullPath))
            {
                var fi = new FileInfo(fullPath);
                sizeAfter = fi.Length;
                modDate = fi.LastWriteTime;
            }

            string extension = Path.GetExtension(fullPath);
            if (extension != null) extension = extension.ToLower();

            EventHandler value = (object s, EventArgs e) =>
            {
                if (!File.Exists(fullPath))
                {
                    MessageBox.Show("Fichier introuvable : " + fullPath, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (var dlg = new DecompressionDialog(fullPath))
                {
                    dlg.Format = extension;
                    var res = dlg.ShowDialog(this);
                    if (res == DialogResult.OK)
                        lblStatus.Text = "Décompression terminée";
                    else
                        lblStatus.Text = "Décompression annulée";
                }
            };
            EventHandler clickHandler = value;

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

            string FormatSize(long bytes)
            {
                if (bytes < 1024) return bytes + " B";
                if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("F1") + " KB";
                if (bytes < 1024 * 1024 * 1024) return (bytes / (1024.0 * 1024.0)).ToString("F1") + " MB";
                return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("F2") + " GB";
            }

            int widthName = 120;
            int widthBefore = 80;
            int widthAfter = 80;
            int widthDate = 100;
            int totalWidth = widthName + widthBefore + widthAfter + widthDate;
            if (flpHistory.ClientSize.Width > totalWidth)
            {
                widthName += flpHistory.ClientSize.Width - totalWidth - 4;
            }

            Label CreateDataLabel(string text, int width)
            {
                return new Label
                {
                    Text = text,
                    Font = new System.Drawing.Font("Segoe UI", 8.25F),
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

            string shortName = displayName.Length > 25 ? displayName.Substring(0, 22) + "..." : displayName;
            var lblName = CreateDataLabel(shortName, widthName);
            var lblSizeBefore = CreateDataLabel(FormatSize(sizeBefore), widthBefore);
            var lblSizeAfter = CreateDataLabel(FormatSize(sizeAfter), widthAfter);
            var lblDate = CreateDataLabel(modDate.ToString("dd/MM/yyyy HH:mm"), widthDate);

            itemPanel.Controls.Add(lblName);
            itemPanel.Controls.Add(lblSizeBefore);
            itemPanel.Controls.Add(lblSizeAfter);
            itemPanel.Controls.Add(lblDate);

            int currentX = 0;
            foreach (Control control in itemPanel.Controls)
            {
                control.Location = new System.Drawing.Point(currentX, 0);
                currentX += control.Width;
            }

            flpHistory.Controls.Add(itemPanel);

            foreach (Control control in itemPanel.Controls)
            {
                control.Click += clickHandler;
            }
        }
    }
}