using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace HW7_PlaneShooting
{
    class CBoss //
    {
        public Point pt = new Point(0, 0);
        public Rectangle rec;
        public int dirX = 0; 
        public int dirY = 10;
        Bitmap bmp;
        public bool isLive = true;
        Image im;
        public int life = 1000;

        public CBoss(Image img, int seed, int a, int b) //建構子
        {
            Random rd = new Random(seed);
            bmp = new Bitmap(img);
            bmp.MakeTransparent(Color.White);
            this.pt.X = rd.Next(img.Width, 400-img.Width);
            this.pt.Y = 0;
            rec = new Rectangle(pt.X, pt.Y ,img.Width, img.Height);
            this.dirX = b;
            this.dirY = 0;
            im = img;
            life = a;
        }

        public void Draw(Graphics g) //畫圖
        {
            if (isLive)
            {
                Graphics dc = Graphics.FromImage(bmp);
                g.DrawImage(bmp, pt);
                dc.Dispose();
            }
        }

        public void Move() //左右移動
        {
            pt.X += dirX;
            rec.X = pt.X;
            rec.Y = pt.Y;
            if (pt.X > 400-im.Width)
            {
                dirX = -dirX;
                pt.X = 400 - im.Width;
                rec.X = pt.X;
                rec.Y = pt.Y;
            }
            else if (pt.X < im.Width-100)
            {
                dirX = -dirX;
                pt.X = im.Width-100;
                rec.X = pt.X;
                rec.Y = pt.Y;
            }
        }

        public bool Checkdie(CBullet bm) //檢查是否被射死
        {
            if (rec.IntersectsWith(bm.rec))
            {
                life -= bm.damage; //生命減子彈的攻擊數
                if (life <= 0)
                {
                    bm.isLive = false;
                    isLive = false;
                    return true;
                }
                bm.isLive = false;
            }
            return false;
        }
    }
}
