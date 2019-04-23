using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopLab3.Models
{
    public class BreakPoint: Element
    {
        public BreakPoint(int x, int y) : this()
        {
            X = x;
            Y = y;
            Width = 50;
            Height = 50;
        }
        public BreakPoint()
        {
            img = ResourceMain.blok;
        }
    }
}
