using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BJ
{
    class Card
    {
        public int value;
        public System.Drawing.Image image;
        public System.Drawing.Image reverse_image;
        public bool reverse;

        //wartosc karty
        public int get_value(int x)
        {
            if (x % 13 == 0)
                return 1;
            if (x % 13 < 10)
                return x % 13 + 1;
            else
                return 10;
        }
    }
}
