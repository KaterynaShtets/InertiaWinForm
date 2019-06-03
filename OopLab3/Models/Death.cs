using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopLab3.Models
{
    class Death:Element
    {
        public Death(int x, int y) : this()
        {
            X = x;
            Y = y;
            Width = 50;
            Height = 50;
        }
        public Death()
        {
            img = ResourceMain.death;
        }
        public void NullLives(Player p)
        {
            p.Lives = 0;
        }
    }
}
