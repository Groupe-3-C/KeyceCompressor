using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyceCompressor.Utils
{
    public static class AnimationHelper
    {
        public static void TypewriterEffect(Label label, string text, int interval = 80)
        {
            label.Text = "";
            int index = 0;
            var timer = new Timer { Interval = interval };
            timer.Tick += (s, e) =>
            {
                if (index < text.Length)
                {
                    label.Text += text[index++];
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        public static void LoadingDots(Label label, int interval = 500)
        {
            int dots = 0;
            var timer = new Timer { Interval = interval };
            timer.Tick += (s, e) =>
            {
                dots = (dots + 1) % 4;
                label.Text = "Chargement" + new string('.', dots);
            };
            timer.Start();
        }
    }
}
