using System.Windows.Forms;
using System;
using KeyceCompressor.Forms;


namespace KeyceCompressor.Forms
{
    public partial class SplashScreen : Form
    {
        // Déclarez les Timers ici si vous ne les avez pas dans le designer
        // private Timer timerTyping;
        // private Timer timerDots;
        // private Timer timerSplash;

        public SplashScreen()
        {
            InitializeComponent(); // méthode générée par le Designer

            // Configuration des contrôles définis par le Designer
            lblWelcome.Text = "";
            lblLoading.Text = "Loading";

            // Effet de saisie pour le titre
            string text = "Keyce Compressor";
            int index = 0;

            // Assurez-vous que ces Timers sont soit dans le designer, soit déclarés ici
            // Si vous les avez dans le designer, décommentez les lignes ci-dessus
            // et supprimez les lignes de déclaration dans le designer.

            timerTyping.Interval = 100;
            timerTyping.Tick += (s, e) =>
            {
                if (index < text.Length) lblWelcome.Text += text[index++];
                else timerTyping.Stop();
            };
            timerTyping.Start();

            // Points qui s'ajoutent à "Loading..."
            int dots = 0;
            timerDots.Interval = 500;
            timerDots.Tick += (s, e) =>
            {
                dots = (dots + 1) % 4;
                lblLoading.Text = "Loading" + new string('.', dots);
            };
            timerDots.Start();

            // Fermeture automatique et ouverture du MainForm
            timerSplash.Interval = 5000; // 5 secondes comme demandé
            timerSplash.Tick += (s, e) =>
            {
                timerTyping.Stop();
                timerDots.Stop();
                timerSplash.Stop();
                Hide();
                var main = new MainForm();
                main.Show(); // C'est la méthode correcte
            };
            timerSplash.Start();
        }
    }
}
