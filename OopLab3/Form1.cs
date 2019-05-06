using OopLab3.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        string name;
        WinForm win = new WinForm();
        Form2 win2 = new Form2();
        Menu m;
        int steps = 0;
        public Form1(Menu m, string _name)
        {
            this.m = m;
            InitializeComponent();
            name = _name;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            field.GenerateWall();
            field.GenerateCoins();
            field.GenerateTeleport();
            field.GenerateDeath();
            field.GenerateKillers();
            field.GenerateHelp();
            Thread myThread = new Thread(MoveEnemys);
            myThread.Start();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            label1.Text = "Lives:" + field.Lives.ToString();
            label2.Text = "You must collect" + field.Prizes.ToString() + "prizes";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Player.img, Player.X, Player.Y, 50, 50);
            DrawWalls();            
            DrawCoins();
            DrawDeath();
            DrawTeleport();
            DrawKiller();
            DrawMedHelp();
            
        }
        private void DrawDeath()
        {
            foreach (Element d in field.place)
            {
                Death death = d as Death;
                if (death != null)
                {
                    g.DrawImage(d.img, d.X, d.Y, d.Width, d.Height);
                }
            }
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
        private void DrawMedHelp()
        {
            foreach (Element b in field.place)
            {
                MedHelp br = b as MedHelp;
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
        private void DrawKiller()
        {
            foreach (Element k in field.place)
            {
                Killer kill = k as Killer;
                if (kill != null)
                {
                    g.DrawImage(k.img, k.X, k.Y, k.Width, k.Height);
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
        public void MinusLives()
        {
            field.Lives--;
        }
        public void NullLives()
        {
            field.Lives = 0;
        }
        public void PlusLives()
        {
            field.Lives++;
        }
        public bool StopGame()
        {
            if (field.Prizes == 0)
            {
                return true;
            }
            return false;
        }
        public bool Fail()
        {
            if (field.Lives == 0)
            {
                return true;
            }
            return false;
        }

        public void MoveEnemys()
        {
            int index = 50;
            int count = 0;
            while (true)
            {
                foreach (var c in field.place)
                {
                    if (c is Death)
                    {
                        int X = c.X;
                        int Y = c.Y;
                        c.Y += index;
                        if (field[c.X, c.Y] is Player)
                        {
                            NullLives();
                            break;
                        }
                        field[c.X, c.Y] = c;
                        field[X, Y] = null;
                        break;
                    }
                }
                count += index;
                if (count == 200 || count == 0)
                {
                    index *= -1;
                }
                Thread.Sleep(300);
            }
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
                if (Player.X + x == 100 && Player.Y + y == 450)
                {
                    Teleport.Teleportation(Player, 350 + x, 50 + y);
                    field.place[Player.X, Player.Y] = Player;
                }
                if (Player.X + x == 350 && Player.Y + y == 50)
                {
                    Teleport.Teleportation(Player, 100 + x, 450 + y);
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
            else if (field.place[Player.X + x, Player.Y + y] is Death)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                NullLives();
                return false;
            }
            else if (field.place[Player.X + x, Player.Y + y] is MedHelp)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                PlusLives();
                return false;
            }
            else if (field.place[Player.X + x, Player.Y + y] is Killer)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                MinusLives();
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
                steps++;
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
                steps++;
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
                steps++;
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
                steps++;
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
                steps++;
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
                steps++;
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
                steps++;
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
                steps++;
                bool check = true;
                while (check)
                {
                    check = MovePlayer(50, 50);
                    Refresh();
                    Thread.Sleep(100);
                }

            }
            label1.Text = field.Lives.ToString();
            label2.Text = field.Prizes.ToString();
            if (StopGame())
            {
                WriteToDB(name, steps, "Win");
                win.Show();
                this.Dispose();
                m.Show();
            }
            if (Fail())
            {
                WriteToDB(name, steps, "Fail");
                win2.Show();
                this.Dispose();
                m.Show();
            }
        }
      
        private void WriteToDB(string name, int steps, string res)
        {
            string connectionString = @"Data Source=DESKTOP-OQ106UV\SQLEXPRESS;Initial Catalog=Lab;Integrated Security=True;Pooling=False";
            string insert = String.Format("INSERT INTO Games (Name, Steps, State) VALUES ('{0}', {1}, '{2}')", name, steps, res);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(insert, connection);//создаем объект комманда
                                                                        //выполняем
                command.ExecuteNonQuery();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
    

