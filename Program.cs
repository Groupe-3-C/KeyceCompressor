using System;
using System.Windows.Forms;
using KeyceCompressor.Forms;

namespace KeyceCompressor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Affiche le splash de façon modale ; le SplashScreen doit appeler Close() quand il a fini.
                using (var splash = new SplashScreen())
                {
                    splash.ShowDialog();
                }

                // Démarre la boucle principale avec le MainForm.
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                // Message simple en cas d'erreur non gérée au démarrage.
                MessageBox.Show("Une erreur est survenue au démarrage : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}