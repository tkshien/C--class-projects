using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace HW7_PlaneShooting
{
    class CBullet
    {
        public Point pt=new Point(200,400);
        public Rectangle rec;
        public int dirY = -10;
        public int dirX = 0;
        Bitmap bmp;
        public bool isLive=true;
        public int damage = 10;

        public CBullet(Image img, Point pt,int a,int b,int c) //子彈的建構子，有BOSS和玩家子彈
        {
            if(a == 1)
                dirY = -10;
            else if(a == 2)
                dirY = 10;
            bmp = new Bitmap(img);
            bmp.MakeTransparent(Color.White);

            Random r = new Random();
            int f = r.Next(0,2);
            if (f == 0)
                dirX = b;
            else
                dirX = -b;
            this.pt.X = pt.X;
            this.pt.Y = pt.Y;
            rec = new Rectangle(pt.X, pt.Y ,img.Width, img.Height);
            damage = c;
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

        public void Move() //子彈向下/上移動（直）
        {
            if (!isLive) return;
            pt.X += dirX;
            pt.Y += dirY;
            rec.X = pt.X;
            rec.Y = pt.Y;
            if(pt.Y<=0 || pt.Y > 600) 
                isLive=false;
        }

        public void Move2() //BOSS 3 子彈撞牆反彈移動
        {
            if (!isLive) return;
            pt.X += dirX;
            pt.Y += dirY;
            rec.X = pt.X;
            rec.Y = pt.Y;

            if (pt.X > 341)
            {
                dirX = -dirX;
                pt.X = 341;
                rec.X = pt.X;
                rec.Y = pt.Y;
            }
            else if (pt.X < 0)
            {
                dirX = -dirX;
                pt.X = 0;
                rec.X = pt.X;
                rec.Y = pt.Y;
            }
            if (pt.Y >600) isLive = false;
        }

        public bool CheckCrash(Rectangle rt) //有沒有射到飛機
        {
            if (rec.IntersectsWith(rt)) return true;
            return false;
        }

        public bool CheckBoom(ArrayList boomList) //有沒有射到子彈
        {
            for (int i = 0; i < boomList.Count; i++)
            {
                CBullet bm = boomList[i] as CBullet;
                if (rec.IntersectsWith(bm.rec))
                {
                    bm.isLive = false;
                    boomList.Remove(bm);
                    isLive = false;
                    return true;
                }
            }
            return false;
        }
    }
}
