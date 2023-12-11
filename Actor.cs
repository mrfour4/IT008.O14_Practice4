using System;
using System.Drawing;
using System.Windows.Forms;

namespace practiceGame
{
    internal class Actor
    {
        Form form;
        PictureBox picture, bum;
        Timer timer;
        int speed;


        public Actor(string imgSrc, int left, int top)
        {
            picture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.AutoSize,
                Image = Image.FromFile(imgSrc),
                BackColor = Color.Transparent,
            };
            picture.Left = left;
            picture.Top = top;


            speed = 6;
        }

        public Actor(string imgSrc, PictureBox player)
        {
            picture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.AutoSize,
                Image = Image.FromFile(imgSrc),
            };


            picture.Left = player.Left + (player.Width / 2) - picture.Width / 2;
            picture.Top = player.Top - picture.Height;
            speed = 30;
        }

        public PictureBox Draw()
        {
            return picture;
        }


        public int getTop()
        {
            return picture.Top;
        }

        public void goBottom()
        {
            picture.Top += speed;
        }

        public void goUp(int left)
        {
            picture.Top = -450;
            picture.Left = left;
        }


        // đạn bắn trúng thiên thạch
        public bool isHit(PictureBox bullet, Form f)
        {
            form = f;
            if (bullet.Bounds.IntersectsWith(picture.Bounds))
            {
                bum = new PictureBox
                {
                    Image = Image.FromFile(@"../../Resources/boom.png"),
                    Width = picture.Width,
                    Height = picture.Height,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Left = picture.Left,
                    Top = bullet.Top,
                };
                timer = new Timer();
                timer.Interval = 100;
                timer.Tick += Timer_Tick;
                timer.Start();
                form.Controls.Add(bum);
                return true;
            }
            return false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            form.Controls.Remove(bum);
            timer.Stop();
        }

        // thiên thạch va chạm với phi thuyền
        public bool isCollide(PictureBox player)
        {
            return player.Bounds.IntersectsWith(picture.Bounds);
        }

        public void setSpeed(int speed)
        {
            this.speed = speed;
        }

        // Đạn bay đến thiên thạch
        public void attack()
        {
            picture.Top -= picture.Height;
        }

    }

}
