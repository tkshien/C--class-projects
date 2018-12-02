using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hw6_FiveStoneChess
{
    public partial class Form1 : Form
    {
        Manage m; //Class
        enum Chess { none, Black, White, Hold }; //棋子
        Chess player = Chess.Black; //玩家棋子顏色
        int computer; //電腦顏色
        int choice = 0, mx = -1, my = -1;
        bool check; //判斷 true false
        int total = 0; //記錄下了多少顆棋子
        int time,minute1,minute2,seconds1,seconds2;

        public Form1()
        {
            InitializeComponent();
            m = new Manage(); //new 一個class
            Player1LB.Text = "玩家一:";  //初始化
            Player2LB.Text = "玩家二:";
            ProgressLB.Text = "請按開始";
            panel2.Visible = false;
            panel3.Visible = false;
            comboBox1.Items.Add(3);
            comboBox1.Items.Add(5);
            comboBox1.Items.Add(10);
            comboBox1.Items.Add(15);
            comboBox1.Items.Add(30);
            comboBox1.Text = "3";
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000;
            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 1000;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) //下棋
        {
            if (choice != 1 && choice != 2) //沒選玩家或電腦
                return;

            if (choice == 1) //玩家對弈
            {
                if (player == Chess.Black) //黑棋
                    check = m.setBlack(e.X, e.Y); //設定黑子
                else
                    check = m.setWhite(e.X, e.Y); //設定白子
                pictureBox1.Invalidate(); //畫圖

                if (check) //下棋成功
                {
                    if (player == Chess.Black) //檢查輸贏
                        check = m.checkWinLose(1);
                    else
                        check = m.checkWinLose(2);

                    if (check) //有贏家
                    {
                        if (player == Chess.Black)
                            ProgressLB.Text = "黑棋勝！";
                        else
                            ProgressLB.Text = "白棋勝！";
                        choice = 0;
                        countdown1(2);
                        countdown2(2);
                        return;
                    }

                    if (player == Chess.Black) //換人
                    {
                        player = Chess.White;
                        ProgressLB.Text = "白棋回合";
                        countdown1(2);
                        countdown2(1);
                    }
                    else
                    {
                        player = Chess.Black;
                        ProgressLB.Text = "黑棋回合";
                        countdown2(2);
                        countdown1(1);
                    }
                    total++; //總棋子+1

                    if (total == 225) //棋盤已滿
                    {
                        ProgressLB.Text = "棋盘已满，本局平棋！";
                        choice = 0;
                        countdown1(2);
                        countdown2(2);
                    }
                }
            }
            else if (choice == 2) //玩家與電腦對弈
            {
                if (player == Chess.Black) //玩家是黑棋
                {
                    check = m.setBlack(e.X, e.Y);
                    computer = 0; //電腦設為白棋
                }
                else
                {
                    check = m.setWhite(e.X, e.Y);
                    computer = 1;
                }
                pictureBox1.Invalidate(); //畫圖

                if (check) //下棋成功
                {
                    if (player == Chess.Black) //檢查輸贏
                        check = m.checkWinLose(1);
                    else
                        check = m.checkWinLose(2);

                    if (check) //有贏家
                    {
                        if (player == Chess.Black)
                            ProgressLB.Text = "黑棋勝！";
                        else
                            ProgressLB.Text = "白棋勝！";
                        choice = 0;
                        return;
                    }
                    total++;

                    if (total == 225) //棋盤已滿
                    {
                        ProgressLB.Text = "棋盘已满，本局平棋！";
                        choice = 0;
                    }
                    else
                    {
                        //電腦下棋
                        if (m.ComputerAIMove(computer)) //呼叫電腦下棋函式
                        {
                            pictureBox1.Invalidate(); //畫圖

                            if (computer == 1) //檢查輸贏
                                check = m.checkWinLose(1);
                            else
                                check = m.checkWinLose(2);

                            if (check) //有贏家
                            {
                                if (computer == 1)
                                    ProgressLB.Text = "黑棋勝！";
                                else
                                    ProgressLB.Text = "白棋勝！";
                                choice = 0;
                                return;
                            }
                            total++;

                            if (total == 225) //棋盤已滿
                            {
                                ProgressLB.Text = "棋盘已满，本局平棋！";
                                choice = 0;
                            }
                        }
                        else //電腦沒步走，玩家勝利
                        {
                            MessageBox.Show("恭喜你，你赢了！棋盘上所有点均为电脑的禁手点！", "本局结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (player == Chess.Black)
                                ProgressLB.Text = "黑棋勝！";
                            else
                                ProgressLB.Text = "白棋勝！";
                            choice = 0;
                            return;
                        }
                    }
                }
            }
        }

        private void StartBTN_Click(object sender, EventArgs e) //開始按鈕
        {
            choice = 0; //判斷玩家對弈還是電腦對弈

            if (radioButton1.Checked == true) //玩家對弈
            {
                choice = 1;

                if (player == Chess.Black) //黑棋
                {
                    Player1LB.Text = "玩家一：黑棋";
                    Player2LB.Text = "玩家二：白棋";
                    ProgressLB.Text = "黑棋回合";
                }
                else //白棋
                {
                    Player1LB.Text = "玩家一：白棋";
                    Player2LB.Text = "玩家二：黑棋";
                    ProgressLB.Text = "白棋回合";
                }
                StartBTN.Enabled = false;
                RegretBTN.Enabled = true;
                panel2.Visible = true;
            }
            else if (radioButton2.Checked == true) //與電腦對弈
            {
                pictureBox1.Enabled = true;
                Random ran = new Random(); //隨機選先手
                int r = ran.Next(2);
                choice = 2;

                if (r == 0)
                    player = Chess.White;

                if (player == Chess.Black) //玩家黑棋
                {
                    Player1LB.Text = "玩家：黑棋";
                    Player2LB.Text = "電腦：白棋";
                }
                else //電腦黑棋
                {
                    Player1LB.Text = "玩家：白棋";
                    Player2LB.Text = "電腦：黑棋";
                    m.setBlack(23 + 7 * 30, 23 + 7 * 30); //電腦先下一棋
                    total++;
                }
                StartBTN.Enabled = false;
                RegretBTN.Enabled = true;
                ProgressLB.Text = "";
            }
        }

        private void ResetBTN_Click(object sender, EventArgs e) //重新按鈕
        {
            pictureBox1.Refresh(); //初始化所有東西
            m.reset();
            total = 0;
            player = Chess.Black;
            StartBTN.Enabled = true;
            RegretBTN.Enabled = false;
            Player1LB.Text = "玩家一:";
            Player2LB.Text = "玩家二:";
            ProgressLB.Text = "請按開始";
            pictureBox1.Invalidate();
            pictureBox1.Enabled = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) //滑鼠移動顯示格子
        {
            if (choice != 1 && choice != 2)
                return;

            //if (player == Chess.Black)
            //    countdown1(1);
            //else
            //    countdown2(1);

            if (mx != -1 || my != -1) //把之前畫的格子去掉
                m.unsetHold(mx, my);
            pictureBox1.Refresh();
            m.setHold(e.X, e.Y); //設定格子
            pictureBox1.Invalidate(); //畫圖
            mx = e.X; //把現在的坐標記下來
            my = e.Y;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) //畫圖事件
        {
            m.Draw(e.Graphics);
        }

        private void RegretBTN_Click(object sender, EventArgs e) //悔棋按鈕
        {
            if (choice != 1 && choice != 2)
                return;

            if (choice == 1) //玩家-一次悔一棋
            {
                if (total != 0)
                {
                    m.regret(1); //去掉最新的棋子
                    total--;

                    pictureBox1.Invalidate(); //畫圖

                    if (player == Chess.Black) //回到上一個玩家
                    {
                        player = Chess.White;
                        ProgressLB.Text = "白棋回合";
                        countdown1(2);
                        countdown2(1);
                    }
                    else
                    {
                        player = Chess.Black;
                        ProgressLB.Text = "黑棋回合";
                        countdown2(2);
                        countdown1(1);
                    }
                }
            }
            else if (choice == 2) //與電腦對弈-一次悔兩棋
            {
                if (total != 0 && total != 1)
                {
                    m.regret(2); //去掉最新的兩個棋子
                    total -= 2;

                    pictureBox1.Invalidate();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            pictureBox1.Enabled = true;
            time = Convert.ToInt32(comboBox1.Text);
            minute1 = minute2 = time;
            seconds1 = seconds2 = 0 ;

            label6.Text = minute1.ToString() + ":" + seconds1.ToString()+"0";
            label7.Text = minute1.ToString() + ":" + seconds1.ToString()+"0";

            if (player == Chess.Black)
                countdown1(1);
            else
                countdown2(1);
        } //倒數計時

        private void countdown1(int a)
        {
            if (a == 1)
                timer1.Start();
            else
                timer1.Stop();
        } //倒數計時玩家一

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds1--;

            if (seconds1 == -1)
            {
                minute1 -= 1;
                seconds1 = 59;
            }
            if (minute1 == 0 && seconds1 == 0)
            {
                timer1.Stop();
                if (player == Chess.Black)
                    ProgressLB.Text = "時間到，白棋勝！";
                else
                    ProgressLB.Text = "時間到，黑棋勝！";
                choice = 0;
                pictureBox1.Enabled = false;
            }
            if(seconds1<10)
            label6.Text = minute1.ToString()+":0"+seconds1.ToString()+"0";
            else
                label6.Text = minute1.ToString() + ":" + seconds1.ToString();
        } //倒數計時 玩家一

        private void countdown2(int a)
        {
            if (a == 1)
                timer2.Start();
            else
                timer2.Stop();
        } //倒數計時玩家二

        private void timer2_Tick(object sender, EventArgs e)
        {
            seconds2--;

            if (seconds2 == -1)
            {
                minute2 -= 1;
                seconds2 = 59;
            }
            if (minute2 == 0 && seconds2 == 0)
            {
                timer2.Stop();

                if (player == Chess.Black)
                    ProgressLB.Text = "時間到，白棋勝！";
                else
                    ProgressLB.Text = "時間到，黑棋勝！";
                choice = 0;
                pictureBox1.Enabled = false;
            }

            if (seconds2 < 10)
                label7.Text = minute2.ToString() + ":0" + seconds2.ToString();
            else
                label7.Text = minute2.ToString() + ":" + seconds2.ToString();
        } //倒數計時玩家二
    }
}
