using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopLab3.Models
{
    class Teleport : Element
    {
        public Teleport(int x, int y) : this()
        {
            X = x;
            Y = y;
            Width = 50;
            Height = 50;
        }
        public Teleport()
        {
            img = ResourceMain.door;
        }
        public static void Teleportation(Player p, int x, int y)
        {
            p.X = x;
            p.Y = y;
        }
    }
}
