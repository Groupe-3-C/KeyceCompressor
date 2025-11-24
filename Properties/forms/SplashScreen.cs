using System;
using System.Windows.Forms;

namespace KeyceCompressor.Forms
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();

            lblWelcome.Text = "";
            lblLoading.Text = "Loading";

            string text = "Keyce Compressor";
            int index = 0;
            timerTyping.Interval = 100;
            timerTyping.Tick += (s, e) =>
            {
                if (index < text.Length) lblWelcome.Text += text[index++];
                else timerTyping.Stop();
            };
            timerTyping.Start();

            int dots = 0;
            timerDots.Interval = 500;
            timerDots.Tick += (s, e) =>
            {
                dots = (dots + 1) % 4;
                lblLoading.Text = "Loading" + new string('.', dots);
            };
            timerDots.Start();

            // 5 secondes exactes :
            timerSplash.Interval = 5000;
            timerSplash.Tick += (s, e) =>
            {
                timerTyping.Stop();
                timerDots.Stop();
                timerSplash.Stop();
                // fermer le splash pour retourner au caller (Program.Main)
                this.Close();
            };
            timerSplash.Start();
        }
    }
}