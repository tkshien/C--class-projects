using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace HW7_PlaneShooting
{
    class CPlane
    {
        public Point pt = new Point(0, 0);
        public Rectangle rec;
        public int dir = 10;
        Bitmap bmp;

        public CPlane(Image img, Point pt) //玩家飛機建構子
        {
            bmp = new Bitmap(img);
            bmp.MakeTransparent(Color.White);
            this.pt.X = pt.X;
            this.pt.Y = pt.Y;
            rec = new Rectangle(pt.X, pt.Y, img.Width, img.Height);
        }

        public void Draw(Graphics g) //畫圖
        {
            Graphics dc = Graphics.FromImage(bmp);
            g.DrawImage(bmp, pt);
            dc.Dispose();

        }

        public void Move(int x) //滑鼠移動
        {
            if (x < pt.X + (bmp.Width / 2)) dir = -10;
            else dir = 10;
        }

        public void Move() //移動
        {
            pt.X += dir;
            if (pt.X + bmp.Width > 400) 
                pt.X = 400 - bmp.Width;
            else if (pt.X < 0) 
                pt.X = 0;
            rec.X = pt.X;
        }

        public void KeyMove(int a) //鍵盤移動
        {
            if (a == 1)
                dir = -10;
            else
                dir = 10;
        }
    }
}
