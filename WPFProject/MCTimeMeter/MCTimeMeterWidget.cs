using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MCTimeMeter
{
    public class MCTimeMeterWidget :Window
    {
        public Canvas canvas = new Canvas();

        public MCTimeMeterWidget() {
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Black;
            this.Topmost = true;

            this.Width = 400;
            this.Height = 300;
            canvas.Width = this.Width;
            canvas.Height = this.Height;
            canvas.Background = Brushes.Black;
            this.Content = canvas;
        }
    }
}
