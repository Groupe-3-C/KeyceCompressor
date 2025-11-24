using System.Windows.Forms;
using System;


namespace KeyceCompressor.Utils
{
    public static class FileDialogHelper
    {
        public static string OpenFile(string filter = "Tous les fichiers|*.*", string title = "Ouvrir")
        {
            using (var ofd = new OpenFileDialog { Filter = filter, Title = title })
            {
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            }
        }

        public static string OpenFolder(string description = "Sélectionnez un dossier")
        {
            using (var fbd = new FolderBrowserDialog { Description = description })
            {
                return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : null;
            }
        }

        public static string SaveKeyceFile(string defaultName = "archive.keyce")
        {
            using (var sfd = new SaveFileDialog
            {
                Filter = "Fichier Keyce|*.keyce",
                FileName = defaultName,
                Title = "Enregistrer sous..."
            })
            {
                return sfd.ShowDialog() == DialogResult.OK ? sfd.FileName : null;
            }
        }
    }
}