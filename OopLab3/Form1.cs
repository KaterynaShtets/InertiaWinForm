using OopLab3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OopLab3
{
    public partial class Form1 : Form
    {
        //private Bitmap PlayerImage = new Bitmap(ResourceMain.hero4);

        public Player Player = new Player() { X = 50, Y = 50 };
        private Field field = new Field(500, 500);
        private Graphics g;
        WinForm win = new WinForm();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            field.GenerateWall();
            field.GenerateCoins();
            field.GenerateTeleport();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Player.img, Player.X, Player.Y, 50, 50);
            DrawWalls();
            DrawCoins();
            DrawTeleport();
        }
        private void DrawWalls()
        {
            foreach (Element b in field.place)
            {
                BreakPoint br = b as BreakPoint;
                if (br != null)
                {
                    g.DrawImage(b.img, b.X, b.Y, b.Width, b.Height);
                }
            }
        }
        private void DrawTeleport()
        {
            foreach (Element b in field.place)
            {
                Teleport br = b as Teleport;
                if (br != null)
                {
                    g.DrawImage(b.img, b.X, b.Y, b.Width, b.Height);
                }
            }
        }
        private void DrawCoins()
        {
            foreach (Element p in field.place)
            {
                Prize pr = p as Prize;
                if (pr != null)
                {
                    g.DrawImage(p.img, p.X, p.Y, p.Width, p.Height);
                }
            }
        }
        public void MinusPrizes()
        {
            field.Prizes--;
        }

        public bool StopGame()
        {
            if (field.Prizes == 0)
            {
                return true;
            }
            return false;
        }
        public bool MovePlayer(int x, int y)
        {
            int X = Player.X;
            int Y = Player.Y;
            
            if (field.place[Player.X + x, Player.Y + y] == null)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                return true;
            }
            else if (field.place[Player.X + x, Player.Y + y] is BreakPoint)
            {
                return false;
            }
            else if (field.place[Player.X + x, Player.Y + y] is Teleport)
            {
                if(Player.X + x==100&&  Player.Y + y == 450)
                {
                    Teleport.Teleportation(Player,350+x,50+y);
                    field.place[Player.X , Player.Y] = Player;
                }
                if (Player.X + x == 350 && Player.Y + y == 50)
                {
                    Teleport.Teleportation(Player, 100+x, 450+y);
                    field.place[Player.X, Player.Y] = Player;
                }
                return true;
            }
            else if (field.place[Player.X + x, Player.Y + y] is Prize)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                MinusPrizes();
                return false;
            }
            else if (field.place[Player.X + x, Player.Y + y] is null)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                return true;
            }

            return false;
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'S' || e.KeyChar == 's')
            {
                bool check = true;
                while (check)
                {
                    if (Player.Y == 450)
                    {
                        check = MovePlayer(0, 49);
                    }
                    else
                    {
                        check = MovePlayer(0, 50);
                    }
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'w' || e.KeyChar == 'W')
            {
                bool check = true;
                while (check)
                {
                    check = MovePlayer(0, -50);
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'a' || e.KeyChar == 'A')
            {
                bool check = true;
                while (check)
                {
                    check = MovePlayer(-50, 0);
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'D' || e.KeyChar == 'd')
            {
                bool check = true;
                while (check)
                {
                    check = MovePlayer(50, 0);
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'q' || e.KeyChar == 'Q')
            {
                bool check = true;
                while (check)
                {
                    check = MovePlayer(-50, -50);
                    Refresh();
                    Thread.Sleep(100);

                }
            }
            else if (e.KeyChar == 'e' || e.KeyChar == 'E')
            {
                bool check = true;
                while (check)
                {  
                    check = MovePlayer(50, -50);
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'Z' || e.KeyChar == 'z')
            {
                bool check = true;
                while (check)
                { 
                    check = MovePlayer(-50, 50);
                    Refresh();
                    Thread.Sleep(100);
                }
            }
            else if (e.KeyChar == 'x' || e.KeyChar == 'X')
            {
                bool check = true;
                while (check)
                {
                    check = MovePlayer(50, 50);
                    Refresh();
                    Thread.Sleep(100);
                }

            }
            if (StopGame())
            {
                win.Show();
                this.Hide();
            }
        }
    }
}
    

