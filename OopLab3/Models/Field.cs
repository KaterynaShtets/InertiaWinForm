using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopLab3.Models
{
    public class Field 
    { 
        public int Width { get; set; }
        public int Height { get; set; }
        public Element[,] place;
        private Bitmap Background;
        public Field(int width, int height)
        {
            Width = width;
            Height = height;
            place = new Element[width, height];
            Background = ResourceMain.blok;
        }
        public Element this[int y, int x]
        {
            set { place[y, x] = value; }
            get { return place[y, x]; }
        }
       
        public void GenerateWall()
        {

            for (int i = 0; i < Width; )
            {
                BreakPoint wallUp = new BreakPoint(i, 0);
                BreakPoint wallDown = new BreakPoint(i, Height - 1);
                place[wallUp.X, wallUp.Y] = wallUp;
                place[wallDown.X, wallDown.Y] = wallDown;
                i += (wallUp.Width);
            }
            for (int i = 0; i < Height; )
            {
                BreakPoint wallLeft = new BreakPoint(0, i);
                BreakPoint wallRight = new BreakPoint(Width - wallLeft.Width, i);
                place[wallLeft.X, wallLeft.Y] = wallLeft;
                place[wallRight.X, wallRight.Y] = wallRight;
                i += (wallLeft.Height);
            }
            for (int i = 0; i < Height;)
            {
                BreakPoint wall = new BreakPoint(150, i);
                place[wall.X, wall.Y] = wall;
                i += (wall.Height+50);
            }
            for (int i = 0; i < Height;)
            {
                BreakPoint wall = new BreakPoint(300, i);
                place[wall.X, wall.Y] = wall;
                i += (wall.Height + 50);
            }
        }

        public void GenerateCoins(Player player)
        {
            for (int i = 0; i < Height;)
            {
                Prize p = new Prize(250, i+50);
                place[p.X, p.Y] = p;
                i += (p.Height+50);
                player.Prizes++;
            }
            Prize p3 = new Prize(400, 450);
            place[p3.X, p3.Y] = p3;
        }
        public void GenerateHelp()
        { 
            MedHelp help = new MedHelp(400, 400);
            place[help.X, help.Y] = help;
        }
        public void GenerateKillers()
        {
            
            Killer killer = new Killer(250, 250);
            place[killer.X, killer.Y] = killer;
            Killer killer2 = new Killer(400, 250);
            place[killer2.X, killer2.Y] = killer2;
          
        }
        public void GenerateDeath()
        {
            
            Death death = new Death(50, 250);
            place[death.X, death.Y] = death;
        }
        public void GenerateTeleport()
        {
            Teleport t=new Teleport(100, 450);
            place[t.X, t.Y] = t;
            Teleport t2 = new Teleport(350, 50);
            place[t2.X, t2.Y] = t2;
        }
        public bool StopGame(Player p)
        {
            if (p.Prizes == 0)
            {
                return true;
            }
            return false;
        }
        public bool Fail(Player p)
        {
            if (p.Lives == 0)
            {
                return true;
            }
            return false;
        }
        
    }
}
