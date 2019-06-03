using OopLab3.Models;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace OopLab3
{
    public partial class Form1 : Form
    {

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
            field.GenerateCoins(Player);
            field.GenerateTeleport();
            field.GenerateDeath();
            field.GenerateKillers();
            field.GenerateHelp();
            Thread myThread = new Thread(MoveEnemys);
            myThread.Start();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            label1.Text = "Lives:" + Player.Lives.ToString();
            label2.Text = "You must collect" + Player.Prizes.ToString() + "prizes";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Player.img, Player.X, Player.Y, 50, 50);
            DrawElements();
            
        }
        
        private void DrawElements()
        {
            foreach (Element p in field.place)
            {
                if(p != null)
                {
                    g.DrawImage(p.img, p.X, p.Y, p.Width, p.Height);
                }            
            }
        }
        

        public void MoveEnemys()
        {
            int index = 50;
            int count = 0;
            while (true)
            {
                foreach (var c in field.place)
                {
                    var d = c as Death;
                    if (d != null)
                    {
                        int X = c.X;
                        int Y = c.Y;
                        c.Y += index;
                        if (field[c.X, c.Y] is Player)
                        {
                            d.NullLives(Player);
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
            var elem = field.place[Player.X + x, Player.Y + y];

            if (elem == null)
            {
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                return true;
            }
            else if (elem is BreakPoint)
            {
                return false;
            }
            else if (elem is Teleport)
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
            else if (elem is Prize)
            {
                var p = elem as Prize;
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                p.MinusPrizes(Player);
                return false;
            }
            else if (elem is Death)
            {
                var d = elem as Death;
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                d.NullLives(Player);
                return false;
            }
            else if (elem is MedHelp)
            {
                var m = elem as MedHelp;
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                m.PlusLives(Player);
                return false;
            }
            else if (elem is Killer)
            {
                var k = elem as Killer;
                Player.Move(x, y);
                field.place[Player.X, Player.Y] = Player;
                field.place[X, Y] = null;
                k.MinusLives(Player);
                return false;
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
            label1.Text = Player.Lives.ToString();
            label2.Text = Player.Prizes.ToString();
            if (field.StopGame(Player))
            {
                WriteToDB(name, steps, "Win");
                win.Show();
                this.Dispose();
                m.Show();
            }
            if (field.Fail(Player))
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
    

