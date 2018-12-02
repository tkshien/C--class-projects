using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace HW7_PlaneShooting
{
    class CEnemy
    {
        public Point pt=new Point(0,0);
        public Rectangle rec;
        public int dirX = 0;
        public int dirY = 10;
        Bitmap bmp;
        public bool isLive = true;

        public CEnemy(Image img, int seed) //敵人小飛機建構子
        {
            Random rd = new Random(seed);
            bmp = new Bitmap(img);
            bmp.MakeTransparent(Color.White);
            this.pt.X = rd.Next(0, 400);
            this.pt.Y = 0;
            rec = new Rectangle(pt.X, pt.Y ,img.Width, img.Height);
            this.dirX = rd.Next(-5,5);
            this.dirY = rd.Next(5,20);
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

        public void Move() //移動，撞牆會反彈
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
            if (pt.Y > 600) isLive = false;
            
        }

        public void Move2() //道具移動
        {
            pt.X += 0;
            pt.Y += 20;
            rec.X = pt.X;
            rec.Y = pt.Y;
            if (pt.Y >= 500) isLive = false;
        }

        public bool CheckBoom(ArrayList boomList) //小飛機撞子彈
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

        public bool CheckCrash(Rectangle rt) //小飛機/道具撞玩家飛機
        {
            if (rec.IntersectsWith(rt)) return true;
            return false;
        }
    }
}
