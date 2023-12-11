using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace practiceGame
{
    public partial class Game : Form
    {
        bool goLeft, goRight, isGameOver;
        int score;
        int playerSpeed = 12;
        Random random = new Random();

        List<Actor> enemys = new List<Actor>();

        List<Actor> bullets = new List<Actor>();

        string EnemyImgSrc = @"../../Resources/meteorite.png";


        public Game()
        {
            InitializeComponent();
            ResetGame();
        }

        void Init()
        {
            ResetUI();
            enemys.Clear();

            int n = 3;
            if (score >= 10)
            {
                n = 5;
            }

            int maxW = this.Width / n;
            int maxH = this.Height / n;

            for (int i = 0; i < n; i++)
            {
                enemys.Add(new Actor(EnemyImgSrc, random.Next((i - 1) * maxW, i * maxW), -1 * random.Next(i * maxH)));
            }

            foreach (var item in enemys)
            {
                this.Controls.Add(item.Draw());
            }

        }

        void EnemysGoBottom()
        {
            foreach (var item in enemys)
            {
                item.goBottom();
            }
        }

        void Hit()
        {
            Actor b = null;
            foreach (var item in enemys)
            {
                foreach (var bullet in bullets)
                {
                    if (item.isHit(bullet.Draw(), this))
                    {
                        score += 1;
                        b = bullet;

                        this.Controls.Remove(bullet.Draw());
                        item.goUp(random.Next(0, this.Width - item.Draw().Width));
                    }
                }

            }
            bullets.Remove(b);
        }

        bool IsPlayerCollideWithEnemy(PictureBox player)
        {
            foreach (var item in enemys)
            {
                if (item.isCollide(player))
                {
                    explosive.Width = item.Draw().Width;
                    explosive.Height = item.Draw().Height;
                    explosive.Left = item.Draw().Left;
                    explosive.Top = item.Draw().Top;
                    player.Image = Image.FromFile(@"../../Resources/boom.png");
                    player.SizeMode = PictureBoxSizeMode.StretchImage;
                    return true;
                }
            }

            return false;
        }

        void BulletActack()
        {
            foreach (var item in bullets)
            {
                item.attack();
            }
        }

        void EnemyGoOut()
        {
            if (bullets.Count == 0) return;
            Actor bullet = null;
            foreach (var item in bullets)
            {
                if (item.getTop() <= 0)
                {
                    bullet = item;
                    this.Controls.Remove(item.Draw());
                }
            }

            bullets.Remove(bullet);
        }

        void EnemysSetSpeed(int speed)
        {
            foreach (var item in enemys)
            {
                item.setSpeed(speed);
            }
        }

        private void ResetGame()
        {
            gameTimer.Start();
            isGameOver = false;

            explosive.Left = -explosive.Width;
            player.Image = Image.FromFile(@"../../Resources/spaceship.png");
            Init();

            score = 0;


            txtScore.Text = "Score: " + score.ToString();
            txtGameOver.Left = -txtGameOver.Width;
        }

        void ResetUI()
        {
            if (enemys.Count == 0) return;
            explosive.Left = -explosive.Width;
            foreach (var item in enemys)
            {
                this.Controls.Remove(item.Draw());
            }

            txtGameOver.Left = -1000;

        }

        private void GameOver()
        {
            gameTimer.Stop();
            isGameOver = true;
            txtGameOver.Left = 0;
            txtGameOver.Text = txtScore.Text + "\nGame over\nPlease, enter to try again";
            txtGameOver.BackColor = Color.Gray;
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

            if (e.KeyCode == Keys.Space && isGameOver == false)
            {
                Actor item = new Actor(@"../../Resources/bullet.png", player);
                bullets.Add(item);
                this.Controls.Add(item.Draw());
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                ResetGame();
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score.ToString();

            EnemysGoBottom();

            foreach (var item in enemys)
            {
                if (item.getTop() >= this.Height)
                {
                    item.goUp(random.Next(0, this.Width - item.Draw().Width));
                }
            }


            if (goLeft && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (goRight && player.Right < this.Width)
            {
                player.Left += playerSpeed;
            }


            BulletActack();

            EnemyGoOut();


            Hit();

            if (IsPlayerCollideWithEnemy(player))
            {
                GameOver();
            }



            if (score == 5)
            {
                EnemysSetSpeed(8);
                playerSpeed = 15;
            }

            if (score == 10)
            {
                EnemysSetSpeed(10);
                playerSpeed = 20;
            }
        }
    }
}
