using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hw6_FiveStoneChess
{
    class Manage
    {
        enum Chess { none, Black, White, Hold }; //棋子
        public enum Result { lose, equal, win }; //結果
        Chess[,] Box = new Chess[15, 15]; //棋盤
        Chess[,] VirtualBox = new Chess[15, 15]; //虛擬棋盤，AI預測用的
        Chess player; // 玩家
        Chess ch = Chess.Black; //電腦
        bool check; //判斷 true false
        Bitmap bd = new Bitmap(Properties.Resources.board); //棋盤圖
        Bitmap b = new Bitmap(Properties.Resources.black); //黑子圖
        Bitmap w = new Bitmap(Properties.Resources.white); //白子圖
        Pen p = new Pen(Color.LightGray, 5); //灰筆
        Stack<Point> pointStack = new Stack<Point>(); //坐標堆疊，悔棋用
        Stack<Chess> colorStack = new Stack<Chess>(); //棋子顏色堆疊，悔棋用
        Stack backTrackStack = new Stack();//用于回溯的栈
        int M = 1;//预测的步数

        public Manage() //建構子，初始化棋盤
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    Box[i, j] = Chess.none; 

        }

        public void Draw(Graphics g) //畫圖
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    if (Box[i, j] == Chess.Black) //畫黑子
                        g.DrawImage(b, 23 + i * 30, 23 + j * 30, 23, 23);
                    else if (Box[i, j] == Chess.White) //畫白子
                        g.DrawImage(w, 23 + i * 30, 23 + j * 30, 23, 23);
                    else if (Box[i, j] == Chess.Hold) //畫灰格子
                        g.DrawRectangle(p, 23 + i * 30, 23 + j * 30, 23, 23);
                }
        }

        public bool checkWinLose(int a) //判斷輸贏，全盤掃描
        {
            int i, j;

            if (a == 1)
                player = Chess.Black;
            else
                player = Chess.White;

            for (i = 0; i < 11; i++) //右下
                for (j = 0; j < 11; j++)
                    if (Box[i, j] == player && Box[i + 1, j + 1] == player && Box[i + 2, j
              + 2] == player && Box[i + 3, j + 3] == player && Box[i + 4, j + 4] == player)
                        return true;

            for (i = 4; i < 15; i++)   //右上 
                for (j = 0; j < 11; j++)
                    if (Box[i, j] == player && Box[i - 1, j + 1] == player && Box[i - 2, j
              + 2] == player && Box[i - 3, j + 3] == player && Box[i - 4, j + 4] == player)
                        return true;

            for (i = 0; i < 15; i++) //直 
                for (j = 4; j < 15; j++)
                    if (Box[i, j] == player && Box[i, j - 1] == player && Box[i, j - 2] == player
              && Box[i, j - 3] == player && Box[i, j - 4] == player)
                        return true;

            for (i = 0; i < 11; i++) //橫 
                for (j = 0; j < 15; j++)
                    if (Box[i, j] == player && Box[i + 1, j] == player && Box[i + 2, j] == player &&
              Box[i + 3, j] == player && Box[i + 4, j] == player)
                        return true;
            return false;
        }

        public bool setBlack(int ex, int ey) //設黑子
        {
            int x = (ex - 23) / 30; //換算坐標
            int y = (ey - 23) / 30;
            if (x < 0 || y < 0 || x >= 15 || y >= 15) //超越邊界
            {
                MessageBox.Show("超边界了");
                return false;
            }
            if (Box[x, y] != Chess.none && Box[x, y] != Chess.Hold) //已經有棋子
                return false;
            if (GetChessValue(Chess.Black, x, y) == -1) //判斷下黑子是否是禁手
            {
                MessageBox.Show("禁手!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            Box[x, y] = Chess.Black; //下黑子
            VirtualBox[x, y] = Chess.Black;
            Point p = new Point(x, y);
            pointStack.Push(p); //放進堆疊
            colorStack.Push(Chess.Black);
            return true;
        }

        public bool setWhite(int ex, int ey) //設定白子
        {
            int x = (ex - 23) / 30; //換算坐標
            int y = (ey - 23) / 30;
            if (x < 0 || y < 0 || x >= 15 || y >= 15) //超越邊界
            {
                MessageBox.Show("超边界了");
                return false;
            }
            if (Box[x, y] != Chess.none && Box[x, y] != Chess.Hold) //已經有棋子
                return false;

            Box[x, y] = Chess.White; //下黑子
            VirtualBox[x, y] = Chess.White;
            Point p = new Point(x, y);
            pointStack.Push(p); //放進堆疊
            colorStack.Push(Chess.White);
            return true;
        }

        public void setHold(int ex, int ey) //設定灰框框
        {
            int x = (ex - 23) / 30;//換算坐標
            int y = (ey - 23) / 30;
            if (x < 0 || y < 0 || x >= 15 || y >= 15) //超越邊界
            {
                return;
            }
            if (Box[x, y] == Chess.none)
                Box[x, y] = Chess.Hold; //下灰框框
        }

        public void unsetHold(int ex, int ey) //去除灰框框
        {
            int x = (ex - 23) / 30;
            int y = (ey - 23) / 30;
            if (x < 0 || y < 0 || x >= 15 || y >= 15)
            {
                return;
            }
            if (Box[x, y] == Chess.Hold)
                Box[x, y] = Chess.none;
        }

        public void reset() //重設
        {
            for (int i = 0; i < 15; i++) //初始化棋盤
                for (int j = 0; j < 15; j++)
                    Box[i, j] = VirtualBox[i, j] = Chess.none;
            pointStack.Clear(); //清空悔棋堆疊
            colorStack.Clear();
        }

        public void regret(int a) //悔棋
        {
            if (a == 1) //玩家對弈
            {
                    Chess color = colorStack.Pop();
                    Point p = pointStack.Pop();

                    Box[p.X, p.Y] = Chess.none; //設定空子
            }
            else //電腦對弈 要 pop 兩次，一顆玩家一顆電腦
            {
                Chess color1 = colorStack.Pop();
                Point p1 = pointStack.Pop();
                Chess color2 = colorStack.Pop();
                Point p2 = pointStack.Pop();
                Box[p1.X, p1.Y] = Chess.none; //設定空子
                VirtualBox[p1.X, p1.Y] = Chess.none;
                Box[p2.X, p2.Y] = Chess.none;
                VirtualBox[p2.X, p2.Y] = Chess.none;
            }
        }

        public class ChessState //棋子点屬性，包括连子数及權值
        {
            public int[] blackConnect = new int[6]; //黑連子

            public int[] blackActive = new int[6]; //黑活三

            public int[] whiteConnect = new int[6]; //白連子

            public int[] whiteActive = new int[6]; //白活三

            public int tempActive3; //活三
        }
        
        public class StackElement//回溯栈元素，AI 預測用
        {
            public int chessColor; //棋子顏色

            public Point[] bestFivePoints = new Point[5]; //最佳五個坐標

            public int pointsCount; //計數

            public int pointNumber; //計數

            public Result[] result = new Result[5]; //五個點的結果

            public int[] stepNumber = new int[5]; //步數
        }

        private int GetChessValue(Chess chesscolor, int x, int y)
        {                 //求(x,y)點的權值

            int totalmarks; //總分

            ChessState Cstate = new ChessState(); //new 一個 ChessState

            Point left, right, top, down, leftTop, rightTop, leftDown, rightDown; //八個方向

            int temp, connectCount;

            //計算八個方向的坐標
            left = new Point(Math.Max(0, x - 4), y);

            right = new Point(Math.Min(14, x + 4), y);

            top = new Point(x, Math.Max(0, y - 4));

            down = new Point(x, Math.Min(14, y + 4));

            temp = Math.Min(x - left.X, y - top.Y);

            leftTop = new Point(x - temp, y - temp);

            temp = Math.Min(x - left.X, down.Y - y);

            leftDown = new Point(x - temp, y + temp);

            temp = Math.Min(right.X - x, y - top.Y);

            rightTop = new Point(x + temp, y - temp);

            temp = Math.Min(right.X - x, down.Y - y);

            rightDown = new Point(x + temp, y + temp);

            if (chesscolor == Chess.Black) //下的是黑子
            {
                if (VirtualBox[x, y] != Chess.none) //已經有棋子
                    return -2;

                else 
                {
                    //處理黑棋連子情況

                    VirtualBox[x, y] = Chess.Black;

                    //左右方向

                    connectCount = ConnectCount(Chess.Black, left, right);

                    Cstate.blackConnect[connectCount]++; //黑連子

                    if (ActiveThree(Chess.Black, connectCount, left, right)) //有黑活三
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //上下方向

                    connectCount = ConnectCount(Chess.Black, top, down);

                    Cstate.blackConnect[connectCount]++;

                    if (ActiveThree(Chess.Black, connectCount, top, down))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //左上 右下方向

                    connectCount = ConnectCount(Chess.Black, leftTop, rightDown);

                    Cstate.blackConnect[connectCount]++;

                    if (ActiveThree(Chess.Black, connectCount, leftTop, rightDown))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //左下 右上方向

                    connectCount = ConnectCount(Chess.Black, leftDown, rightTop);

                    Cstate.blackConnect[connectCount]++;

                    if (ActiveThree(Chess.Black, connectCount, leftDown, rightTop))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    VirtualBox[x, y] = Chess.none;

           
                    //處理白棋連子情況

                    VirtualBox[x, y] = Chess.White;

                    //左右方向

                    connectCount = ConnectCount(Chess.White, left, right);

                    Cstate.whiteConnect[connectCount]++;

                    if (BreakActiveThree(Chess.White, connectCount, x, y, left, right))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //上下方向

                    connectCount = ConnectCount(Chess.White, top, down);

                    Cstate.whiteConnect[connectCount]++;

                    if (BreakActiveThree(Chess.White, connectCount, x, y, top, down))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //左上_右下方向

                    connectCount = ConnectCount(Chess.White, leftTop, rightDown);

                    Cstate.whiteConnect[connectCount]++;

                    if (BreakActiveThree(Chess.White, connectCount, x, y, leftTop, rightDown))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //左下_右上方向

                    connectCount = ConnectCount(Chess.White, leftDown, rightTop);

                    Cstate.whiteConnect[connectCount]++;

                    if (BreakActiveThree(Chess.White, connectCount, x, y, leftDown, rightTop))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    if (ActiveThree(Chess.White, 3, left, right) && ConnectCount(Chess.White, left, right) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.White, 3, top, down) && ConnectCount(Chess.White, top, down) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.White, 3, leftTop, rightDown) && ConnectCount(Chess.White, leftTop, rightDown) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.White, 3, leftDown, rightTop) && ConnectCount(Chess.White, leftDown, rightTop) <= 3) Cstate.tempActive3++;

                    VirtualBox[x, y] = Chess.none;

                    //開始求權值


                    if (Cstate.blackActive[3] > 1 || Cstate.blackActive[4] > 1 || SixConnect(x, y)) //禁手

                        return -1;

                    else if (Cstate.blackConnect[5] > 0) //黑五連子

                        return 150000;

                    else if (Cstate.whiteConnect[5] > 0) //白五連子

                        return 140000;

                    else if (Cstate.blackActive[4] > 0 || Cstate.blackConnect[4] > 1) //黑連四/黑四子

                        return 130000;

                    else if (Cstate.blackConnect[4] == 1 && Cstate.blackActive[3] == 1) //黑四子/黑活三

                        return 120000;

                    else if (Cstate.blackConnect[4] == 1 && Cstate.blackConnect[3] > 0)//黑四子/黑三字

                        return 110000;

                    else if (Cstate.whiteActive[4] > 0 || Cstate.whiteConnect[4] > 1) //白連四/白四子

                        return 100000;

                    else if (Cstate.whiteConnect[4] == 1 && Cstate.tempActive3 == 1) 

                        return 90000;

                    else if (Cstate.whiteActive[3] > 1)

                        return 80000;

                    else if (Cstate.whiteConnect[4] == 1 && Cstate.whiteConnect[3] > 0)

                        return 70000;

                    else
                    {

                        totalmarks = (Cstate.blackConnect[4] + Cstate.blackActive[3]) * 6250 + (Cstate.blackConnect[3] + Cstate.blackActive[2] + Cstate.whiteConnect[4] + Cstate.whiteActive[3]) * 1250

                        + (Cstate.blackConnect[2] + Cstate.whiteConnect[3] + Cstate.whiteActive[2]) * 250 + Cstate.blackActive[1] * 50 + (Cstate.blackConnect[1] + Cstate.whiteConnect[2] + Cstate.whiteActive[1]) * 10 + Cstate.whiteConnect[1] * 2;

                        return totalmarks; //回傳總分

                    }

                }

            }

            else //下白棋
            {

                if (VirtualBox[x, y] != Chess.none)

                    return -2;

                else
                {

                    //處理黑子連子情況

                    VirtualBox[x, y] = Chess.Black;

                    //左右方向

                    connectCount = ConnectCount(Chess.Black, left, right);

                    Cstate.blackConnect[connectCount]++;

                    if (BreakActiveThree(Chess.Black, connectCount, x, y, left, right))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //上下方向

                    connectCount = ConnectCount(Chess.Black, top, down);

                    Cstate.blackConnect[connectCount]++;

                    if (BreakActiveThree(Chess.Black, connectCount, x, y, top, down))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //左上 右下方向

                    connectCount = ConnectCount(Chess.Black, leftTop, rightDown);

                    Cstate.blackConnect[connectCount]++;

                    if (BreakActiveThree(Chess.Black, connectCount, x, y, leftTop, rightDown))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    //左下 右上方向

                    connectCount = ConnectCount(Chess.Black, leftDown, rightTop);

                    Cstate.blackConnect[connectCount]++;

                    if (BreakActiveThree(Chess.Black, connectCount, x, y, leftDown, rightTop))
                    {

                        Cstate.blackConnect[connectCount]--;

                        Cstate.blackActive[connectCount]++;

                    }

                    if (ActiveThree(Chess.Black, 3, left, right) && ConnectCount(Chess.Black, left, right) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.Black, 3, top, down) && ConnectCount(Chess.Black, top, down) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.Black, 3, leftTop, rightDown) && ConnectCount(Chess.Black, leftTop, rightDown) <= 3) Cstate.tempActive3++;

                    if (ActiveThree(Chess.Black, 3, leftDown, rightTop) && ConnectCount(Chess.Black, leftDown, rightTop) <= 3) Cstate.tempActive3++;

                    VirtualBox[x, y] = Chess.none;

                    //處理白子連子情況

                    if (x == 6 && y == 9)

                        x = 6;

                    VirtualBox[x, y] = Chess.White;

                    //左右方向

                    connectCount = ConnectCount(Chess.White, left, right);

                    Cstate.whiteConnect[connectCount]++;

                    if (ActiveThree(Chess.White, connectCount, left, right))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //上下方向

                    connectCount = ConnectCount(Chess.White, top, down);

                    Cstate.whiteConnect[connectCount]++;

                    if (ActiveThree(Chess.White, connectCount, top, down))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //左上 右下方向

                    connectCount = ConnectCount(Chess.White, leftTop, rightDown);

                    Cstate.whiteConnect[connectCount]++;

                    if (ActiveThree(Chess.White, connectCount, leftTop, rightDown))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    //左下 右上方向

                    connectCount = ConnectCount(Chess.White, leftDown, rightTop);

                    Cstate.whiteConnect[connectCount]++;

                    if (ActiveThree(Chess.White, connectCount, leftDown, rightTop))
                    {

                        Cstate.whiteConnect[connectCount]--;

                        Cstate.whiteActive[connectCount]++;

                    }

                    VirtualBox[x, y] = Chess.none;

                    //開始求權值


                    bool BlackForbiden = (Cstate.tempActive3 > 1 || Cstate.blackActive[4] > 1 || SixConnect(x, y)); //判斷黑子是否禁手

                    if (Cstate.whiteConnect[5] > 0)

                        return 150000;

                    else if (Cstate.blackConnect[5] > 0 && !BlackForbiden)

                        return 140000;

                    else if (Cstate.whiteActive[4] > 0 || Cstate.whiteConnect[4] > 1)

                        return 130000;

                    else if (Cstate.whiteConnect[4] == 1 && Cstate.whiteActive[3] > 0)

                        return 120000;

                    else if (Cstate.blackActive[4] == 1 && !BlackForbiden || Cstate.blackConnect[4] > 1 && !BlackForbiden)

                        return 110000;

                    else if (Cstate.whiteConnect[4] == 1 && Cstate.whiteConnect[3] > 0)

                        return 100000;

                    else if (Cstate.blackConnect[4] > 0 && Cstate.tempActive3 == 1 && !BlackForbiden)

                        return 90000;

                    else if (Cstate.whiteActive[3] > 1)

                        return 80000;

                    else if (Cstate.blackConnect[4] > 0 && Cstate.blackConnect[3] > 0 && !BlackForbiden)

                        return 70000;

                    else
                    {
                        totalmarks = (Cstate.whiteConnect[4] + Cstate.whiteActive[3]) * 6250 + (Cstate.whiteConnect[3] + Cstate.whiteActive[2] + Cstate.blackConnect[4] + Cstate.blackActive[3]) * 1250

                        + (Cstate.whiteConnect[2] + Cstate.blackConnect[3] + Cstate.blackActive[2]) * 250 + Cstate.whiteActive[1] * 50 + (Cstate.whiteConnect[1] + Cstate.blackConnect[2] + Cstate.blackActive[1]) * 10 + Cstate.blackConnect[1] * 2;

                        return totalmarks;
                    }
                }

            }

        }

        private bool ActiveThree(Chess chesscolor, int count, Point point1, Point point2) //判斷兩點之間是否有活三
        {
            int x, y, i, j, length, xPlus = 0, yPlus = 0, sum;

            int temp1, temp2;

            Chess ch;

            if (chesscolor == Chess.Black)
                ch = Chess.White;
            else
                ch = Chess.Black;

            //計算坐標

            temp1 = Math.Min(Math.Min(Math.Min(5 - count, point1.X), point1.Y), 14 - point1.Y);

            temp2 = Math.Min(Math.Min(Math.Min(5 - count, 14 - point2.X), 14 - point2.Y), point2.Y);

            length = Math.Max(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y)) + 1 + temp1 + temp2;

            if (point1.X != point2.X) 
                xPlus = 1;

            if (point1.Y != point2.Y) 
                yPlus = (point2.Y - point1.Y) / Math.Abs(point2.Y - point1.Y);

            for (i = 0; i < length - 4; i++)
            {
                x = point1.X - temp1 * xPlus + i * xPlus;

                y = point1.Y - temp1 * yPlus + i * yPlus;

                if (x + 4 * xPlus > 14 || y + 4 * yPlus > 14) //超越範圍
                    break;
                sum = 0;

                for (j = 0; j < 4; j++)
                {
                    if (VirtualBox[x + j * xPlus, y + j * yPlus] == chesscolor)
                        sum++;

                    else if (VirtualBox[x + j * xPlus, y + j * yPlus] == ch)
                    {
                        sum = 0;
                        break;
                    }
                }

                if (0 < x && 0 <= y - yPlus && y - yPlus <= 14)
                {
                    if (sum == count && VirtualBox[x - xPlus, y - yPlus] == Chess.none && VirtualBox[x + 4 * xPlus, y + 4 * yPlus] == Chess.none)
                        return true; //有活三
                }
            }
            return false; //沒活三
        }

        private bool BreakActiveThree(Chess chesscolor, int count, int x, int y, Point point1, Point point2) //判斷是否需要破壞活三
        {      
            Chess ch;

            if (chesscolor == Chess.Black)
                ch = Chess.White;
            else
                ch = Chess.Black;


            if (!ActiveThree(chesscolor, count, point1, point2)) 
                return false;

            if (count == 5) 
                return false;

            else if (count == 4) 
                return true;

            else
            {
                bool check;

                VirtualBox[x, y] = ch;

                check = !ActiveThree(chesscolor, count - 1, point1, point2);

                VirtualBox[x, y] = chesscolor;

                return check;

            }

        }

        private int ConnectCount(Chess chesscolor, Point point1, Point point2) 
        {
            //求兩點之間可能形成五連子的連子數

            int x, y, i, j, length, xPlus = 0, yPlus = 0, sum, maxSum = 0;

            Chess ch;

            if (chesscolor == Chess.Black)
                ch = Chess.White;
            else
                ch = Chess.Black;

            length = Math.Max(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y)) + 1;

            if (point1.X != point2.X) 
                xPlus = 1;

            if (point1.Y != point2.Y) 
                yPlus = (point2.Y - point1.Y) / Math.Abs(point2.Y - point1.Y);

            for (i = 0; i < length - 4; i++)
            {
                x = point1.X + i * xPlus;

                y = point1.Y + i * yPlus;

                sum = 0;

                for (j = 0; j < 5; j++)
                {
                    if (VirtualBox[x + j * xPlus, y + j * yPlus] == chesscolor)
                        sum++;
                    else if (VirtualBox[x + j * xPlus, y + j * yPlus] == ch)
                    {
                        sum = 0;
                        break;
                    }
                }
                if (maxSum < sum)
                    maxSum = sum;
            }
            return maxSum;
        }

        private bool SixConnect(int x, int y) //判斷長連禁手
        {
            Point left, right, top, down, leftTop, rightTop, leftDown, rightDown;

            int temp;

            Chess temp2 = VirtualBox[x, y];

            VirtualBox[x, y] = Chess.Black;

            bool check;

            left = new Point(Math.Max(0, x - 5), y);

            right = new Point(Math.Min(14, x + 5), y);

            top = new Point(x, Math.Max(0, y - 5));

            down = new Point(x, Math.Min(14, y + 5));

            temp = Math.Min(x - left.X, y - top.Y);

            leftTop = new Point(x - temp, y - temp);

            temp = Math.Min(x - left.X, down.Y - y);

            leftDown = new Point(x - temp, y + temp);

            temp = Math.Min(right.X - x, y - top.Y);

            rightTop = new Point(x + temp, y - temp);

            temp = Math.Min(right.X - x, down.Y - y);

            rightDown = new Point(x + temp, y + temp);

            check = SixConnectResult(left, right) || SixConnectResult(top, down) || SixConnectResult(leftTop, rightDown) || SixConnectResult(leftDown, rightTop);

            VirtualBox[x, y] = temp2;

            return check;
        }

        private bool SixConnectResult(Point point1, Point point2) //判斷兩點之間是否能形成長連禁手
        {
            int x, y, i, j, length, xPlus = 0, yPlus = 0, sum;

            length = Math.Max(Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y)) + 1;

            if (point1.X != point2.X) xPlus = 1;

            if (point1.Y != point2.Y) yPlus = (point2.Y - point1.Y) / Math.Abs(point2.Y - point1.Y);

            for (i = 0; i < length - 5; i++)
            {
                x = point1.X + i * xPlus;

                y = point1.Y + i * yPlus;

                sum = 0;

                for (j = 0; j < 6; j++)
                {
                    if (VirtualBox[x + j * xPlus, y + j * yPlus] == Chess.Black)
                        sum++;

                    else
                    {
                        sum = 0;
                        break;
                    }
                }
                if (sum == 6) 
                    return true;
            }
            return false;
        }

        public bool ComputerAIMove(int com) //電腦下棋
        {
            Point bestPoint = new Point(); //最佳下棋坐標
            Chess computer; //電腦顏色

            if (com == 0)
                computer = Chess.White;
            else
                computer = Chess.Black;

            if (FindBestPoint(ref bestPoint, computer)) //找最佳坐標
            {
                if (computer == Chess.Black) //設定黑子
                    check = setBlack(23 + bestPoint.X * 30, 23 + bestPoint.Y * 30);
                else //設定白子
                    check = setWhite(23 + bestPoint.X * 30, 23 + bestPoint.Y * 30);
                return true;
            }
            return false;
        }

        private bool FindBestPoint(ref Point bestPoint, Chess computercolor) //找最佳坐標
        {
            Result finalresult = Result.lose; //預設最終結果

            int i, bestStepNumber = 0;

            StackElement tempStackElement = new StackElement(); //預測的堆疊

            if (!FindBestFivePoints(computercolor, ref tempStackElement)) //沒有最佳坐標
                return false;

            backTrackStack.Push(tempStackElement); //把坐標放入堆疊

            while (backTrackStack.Count > 0)//堆疊非空
            {
                tempStackElement = (StackElement)backTrackStack.Pop(); 

                if (tempStackElement.pointNumber < tempStackElement.pointsCount)
                {
                    Chess temp;
                    //虚拟棋盘上下一棋
                    if (tempStackElement.chessColor == 1)
                        temp = Chess.Black;
                    else
                        temp = Chess.White;

                    VirtualBox[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = temp;

                    if (Win(temp, tempStackElement.bestFivePoints[tempStackElement.pointNumber])) //判斷下了是否會贏
                    {    //贏棋就不繼續預測

                        tempStackElement.result[tempStackElement.pointNumber] = Result.win;

                        tempStackElement.stepNumber[tempStackElement.pointNumber] = backTrackStack.Count + 1;

                        //在虛擬棋盤上退一棋

                        VirtualBox[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = Chess.none;

                        tempStackElement.pointNumber++;

                        backTrackStack.Push(tempStackElement);
                    }
                    else if (backTrackStack.Count == M - 1)
                    {   //堆疊已滿，不繼續預測

                        tempStackElement.result[tempStackElement.pointNumber] = Result.equal;

                        tempStackElement.stepNumber[tempStackElement.pointNumber] = M;

                        //在虛擬棋盤上退一棋

                        VirtualBox[tempStackElement.bestFivePoints[tempStackElement.pointNumber].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber].Y] = Chess.none;

                        tempStackElement.pointNumber++;

                        backTrackStack.Push(tempStackElement);

                    }

                    else //繼續下棋向下預測
                    {     
                        if (tempStackElement.chessColor == 0)
                            ch = Chess.White;
                        else if (tempStackElement.chessColor == 1)
                            ch = Chess.Black;

                        tempStackElement.pointNumber++;

                        backTrackStack.Push(tempStackElement);

                        FindBestFivePoints(ch, ref tempStackElement);

                        backTrackStack.Push(tempStackElement);
                    }
                }
                else //堆疊裡已空或點完全試過
                {
                    if (tempStackElement.pointsCount == 0)//堆疊已空
                    {
                        tempStackElement = (StackElement)backTrackStack.Pop();

                        tempStackElement.result[tempStackElement.pointNumber - 1] = Result.win;

                        tempStackElement.stepNumber[tempStackElement.pointNumber - 1] = backTrackStack.Count + 1;

                        //在虛擬棋盤上退一棋

                        VirtualBox[tempStackElement.bestFivePoints[tempStackElement.pointNumber - 1].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber - 1].Y] = Chess.none;

                        backTrackStack.Push(tempStackElement);

                    }
                    else//堆疊裡的點都已試過
                    {
                        //尋找堆疊裡的點的最好的結果

                        finalresult = tempStackElement.result[0];

                        for (i = 0; i < tempStackElement.pointsCount; i++)

                            if (finalresult < tempStackElement.result[i])

                                finalresult = tempStackElement.result[i];

                        //尋找最佳步數

                        if (finalresult == Result.win)
                        {

                            bestStepNumber = M + 2;

                            for (i = 0; i < tempStackElement.pointsCount; i++)

                                if (finalresult == tempStackElement.result[i] && bestStepNumber > tempStackElement.stepNumber[i])

                                    bestStepNumber = tempStackElement.stepNumber[i];

                        }

                        else 
                        {
                            bestStepNumber = 0;

                            for (i = 0; i < tempStackElement.pointsCount; i++)

                                if (finalresult == tempStackElement.result[i] && bestStepNumber < tempStackElement.stepNumber[i])

                                    bestStepNumber = tempStackElement.stepNumber[i];
                        }
                        if (backTrackStack.Count > 0) //堆疊非空
                        {
                            tempStackElement = (StackElement)backTrackStack.Pop();

                            tempStackElement.result[tempStackElement.pointNumber - 1] = (Result)(0 - finalresult);

                            tempStackElement.stepNumber[tempStackElement.pointNumber - 1] = bestStepNumber;

                            //在虛擬棋盤上退一棋

                            VirtualBox[tempStackElement.bestFivePoints[tempStackElement.pointNumber - 1].X, tempStackElement.bestFivePoints[tempStackElement.pointNumber - 1].Y] = Chess.none;

                            backTrackStack.Push(tempStackElement);
                        }
                    }
                }
            }

            //堆疊已空

            for (i = 0; i < tempStackElement.pointsCount; i++)
                if (finalresult == tempStackElement.result[i] && bestStepNumber == tempStackElement.stepNumber[i])
                    break;

            bestPoint = tempStackElement.bestFivePoints[i];

            return true;
        }

        private bool FindBestFivePoints(Chess chessColor, ref StackElement tempStackElement)
        {   //尋找5個最佳點

            int[,] marks = new int[15, 15];

            bool found;

            int x, y, i, max;

            tempStackElement.pointsCount = 0;

            for (x = 0; x < 15; x++)
                for (y = 0; y < 15; y++)
                    marks[x, y] = GetChessValue(chessColor, x, y);

            for (i = 0; i < 5; i++) //求5個最佳點
            {
                max = 0;
                for (x = 0; x < 15; x++)
                    for (y = 0; y < 15; y++)
                        if (max < marks[x, y])
                            max = marks[x, y];
                for (x = 0; x < 15; x++)
                {
                    found = false;
                    for (y = 0; y < 15; y++)
                        if (max == marks[x, y])
                        {
                            tempStackElement.bestFivePoints[i] = new Point(x, y);

                            tempStackElement.pointsCount++;

                            marks[x, y] = -1;

                            found = true;

                            break;
                        }
                    if (found) //找到就跳出來
                        break;
                }
            }

            if (tempStackElement.pointsCount == 0)
                return false;
            else
            {
                if (chessColor == Chess.Black)
                    tempStackElement.chessColor = 1;
                else
                    tempStackElement.chessColor = 0;

                tempStackElement.pointNumber = 0;

                return true;
            }
        }

        private bool Win(Chess chessColor, int x, int y) //判斷輸贏
        {
            bool check;

            VirtualBox[x, y] = Chess.none;

            if (GetChessValue(chessColor, x, y) >= 150000)
                check = true;
            else
                check = false;

            VirtualBox[x, y] = chessColor;

            return check;
        }

        private bool Win(Chess chessColor, Point point) //判斷輸贏
        {

            return Win(chessColor, point.X, point.Y);

        }
    }
}
