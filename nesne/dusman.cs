using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nesne
{
    public class dusman : Karakter
    {
        public dusman() : base() { }

        public dusman(int can, int x, int y) : base(can, x, y) { }

        public void BombaAt(PictureBox bomba, PictureBox dusmanBox)
        {
            bomba.Left = dusmanBox.Left + dusmanBox.Width / 2 - bomba.Width / 2;
            bomba.Top = dusmanBox.Top + dusmanBox.Height;
            bomba.Visible = true;
        }
    }

}

