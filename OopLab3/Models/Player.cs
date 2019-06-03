using OopLab3.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OopLab3;
namespace OopLab3
{
   public  class Player : Element, IMoveable
    {
        public int Prizes { get; set; }
        public int Lives { get; set; } = 3;
        public Player()
        {
            Height = 50;
            Width = 50;
            img = ResourceMain.hero4;
        }
    
        public void Move(int x, int y) 
        {
            X += x;
            Y += y;            
        }
    }
}
