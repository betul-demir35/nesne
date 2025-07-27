using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nesne
{
    public class Karakter
    {
        public int Can { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Karakter() { }

        public Karakter(int can, int x, int y)
        {
            this.Can = can;
            this.X = x;
            this.Y = y;
        }
    }

}
