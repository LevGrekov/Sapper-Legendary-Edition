using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapper_Legendary_Edition
{
    public class Cell : PictureBox
    {
        public int bombsAraund { get; set; }

        public bool Enable { get; set; } = true;

        public bool Marked { get; set; }
    }
}
