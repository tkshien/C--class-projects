using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW7_PlaneShooting
{
    public partial class Form1 : Form
    {
        CPlane airplane; //玩家飛機
        CBoss boss, boss1, boss2, boss3; //三關的BOSS
        ArrayList bulletList = new ArrayList(); //玩家子彈
        ArrayList bossbullet = new ArrayList(); //BOSS 子彈
        ArrayList enemyList = new ArrayList(); //敵人小飛機
        Random rd = new Random();
        bool isGameOver = true, isGameCleared = false, isBoss = false; 
        SoundPlayer laser, explosion;
        int score, stage = 1, highscore = 0, stbn = 0;
        int gametime = 20; //BOSS出場時間
        int bullettype = 0; //子彈的種類
        int changebullet = 0; //是否出道具
        int bulletamount = 0; //玩家子彈個數
        bool isItem = false; //是否已經有道具掉落
        bool setboss = false; //設定BOSS
        CEnemy item = new CEnemy(Properties.Resources.item, 1); 


        public Form1() //建構子
        {
            InitializeComponent();
            airplane = new CPlane(Properties.Resources.bblueplane, new Point(200, 500));
            boss = new CBoss(Properties.Resources.bboss1, 10, 200, 5);
            boss1 = new CBoss(Properties.Resources.bboss1, 10, 200, 5);
            boss2 = new CBoss(Properties.Resources.bboss2, 10, 400, 10);
            boss3 = new CBoss(Properties.Resources.bboss3, 10, 600, 15);
            laser = new SoundPlayer(Properties.Resources.bulletsound);
            explosion = new SoundPlayer(Properties.Resources.explodesound);
            ReturnBtn.Visible = false;
            ReturnBtn.Enabled = false;
            label2.Visible = false;
            label3.Visible = false;
            comboBox1.Items.Add("Easy");
            comboBox1.Items.Add("Normal");
            comboBox1.Items.Add("Hard");
            comboBox1.Text = "Normal";

        }

        private void Form1_Paint(object sender, PaintEventArgs e) //畫圖
        {
            if(stbn == 1)
            e.Graphics.DrawString("Score : " + score, new Font("標楷體", 24), Brushes.Red, new Point(0, 0));

            if (isGameOver)
            {
                if (stbn == 0)
                {
                    if (score >= highscore) //設定 HighScore
                        highscore = score;
                    e.Graphics.DrawString("Highscore : " + highscore, new Font("標楷體", 24), Brushes.Red, new Point(0, 0));
                }
                timer1.Stop();
                if (isGameCleared)
                {
                    isGameCleared = false;
                }
                return;
            }
            airplane.Draw(e.Graphics);
            if (isBoss)
            {
                boss.Draw(e.Graphics);
                e.Graphics.DrawString("Boss : " + boss.life, new Font("標楷體", 24), Brushes.Red, new Point(200, 0));
            }
            for (int i = 0; i < bulletList.Count; i++)
            {
                CBullet bm = bulletList[i] as CBullet;
                bm.Draw(e.Graphics);
            }
            for (int i = 0; i < enemyList.Count; i++)
            {
                CEnemy em = enemyList[i] as CEnemy;
                em.Draw(e.Graphics);
            }
            for (int i = 0; i < bossbullet.Count; i++)
            {
                CBullet bb = bossbullet[i] as CBullet;
                bb.Draw(e.Graphics);
            }
            if(item.isLive == true)
            item.Draw(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isGameOver == false) //遊戲是否已結束
            {
                if (bulletList.Count > bulletamount) return; //玩家子彈是否已達上限

                CBullet bullet;

                if (bullettype == 0) //攻擊10的子彈
                    bullet = new CBullet(Properties.Resources.bullet, new Point(0, 0), 1, 0, 10);
                else 
                    bullet = new CBullet(Properties.Resources.bullet2, new Point(0, 0), 1, 0, 20); //攻擊20的子彈

                bullet.pt.X = airplane.pt.X + (airplane.rec.Width / 2) - (bullet.rec.Width / 2);
                bullet.pt.Y = airplane.pt.Y - bullet.rec.Height;
                bullet.isLive = true;
                bulletList.Add(bullet);
                this.Invalidate();
                laser.Play();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) //滑鼠移動
        {
            airplane.Move(e.X);
            this.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int test = rd.Next(0, 100);

            if (isBoss) //BOSS 出場
            {
                if (setboss == false) //設定BOSS等級
                {
                    if (stage == 1)
                        boss = boss1;
                    else if (stage == 2)
                        boss = boss2;
                    else
                        boss = boss3;
                    setboss = true;
                }

                if (stage == 1) //第一級BOSS
                {
                    if (test < 10)
                    {
                        CBullet bullet;
                        bullet = new CBullet(Properties.Resources.bossbullet, new Point(0, 0), 2, 0, 10);
                        bullet.pt.X = boss.pt.X + (boss.rec.Width / 2) - (bullet.rec.Width / 2);
                        bullet.pt.Y = boss.pt.Y + bullet.rec.Height;
                        bullet.isLive = true;
                        bossbullet.Add(bullet);
                    }
                }
                else if (stage == 2) //第二級BOSS
                {
                    if (test < 10)
                    {
                        CBullet bullet;
                        bullet = new CBullet(Properties.Resources.bossbullet2, new Point(0, 0), 2, 0, 10);
                        bullet.pt.X = boss.pt.X + (boss.rec.Width / 2) - (bullet.rec.Width / 2);
                        bullet.pt.Y = boss.pt.Y + bullet.rec.Height;
                        bullet.isLive = true;
                        bossbullet.Add(bullet);
                    }
                }

                else if (stage == 3) //第三級 BOSS 
                {
                    if (test < 10)
                    {
                        CBullet bullet;
                        bullet = new CBullet(Properties.Resources.bossbullet3, new Point(0, 0), 2, 5, 10);
                        bullet.pt.X = boss.pt.X + (boss.rec.Width / 2) - (bullet.rec.Width / 2);
                        bullet.pt.Y = boss.pt.Y + bullet.rec.Height;
                        bullet.isLive = true;
                        bossbullet.Add(bullet);
                    }
                }

                enemyList.Clear(); //清空敵人小飛機
                airplane.Move(); //移動
                boss.Move(); //移動


                for (int i = 0; i < bulletList.Count; i++) //玩家子彈
                {
                    CBullet bm = bulletList[i] as CBullet;
                    bm.Move();

                    if (boss.Checkdie(bm)) //打到BOSS
                    {
                        score += 10;
                        bossbullet.Clear();
                        enemyList.Clear();
                        bulletList.Clear();

                        if (stage == 1) //進入下一關
                            this.BackgroundImage = Properties.Resources.bg2;
                        else if (stage == 2)
                            this.BackgroundImage = Properties.Resources.bg3;
                        else if (stage == 3)
                        {
                            isGameOver = true;
                            ReturnBtn.Visible = true;
                            ReturnBtn.Enabled = true;
                            label3.Visible = true;
                            isGameCleared = true;
                        }
                        stage++;
                        gametime = 20; //重設BOSS出場時間
                        isBoss = false;
                        setboss = false;
                    }
                    if (!bm.isLive) bulletList.Remove(bm);
                }

                for (int i = 0; i < bossbullet.Count; i++) //BOSS 子彈
                {
                    CBullet bb = bossbullet[i] as CBullet;

                    if (stage == 3)
                        bb.Move2();
                    else
                        bb.Move();
                    if (bb.CheckCrash(airplane.rec)) //是否打到玩家
                    {
                        explosion.Play();
                        isGameOver = true;
                        label2.Visible = true;
                        ReturnBtn.Visible = true;
                        ReturnBtn.Enabled = true;
                    }
                    if (bb.CheckBoom(bulletList)) //是否打到玩家子彈
                    {
                        bossbullet.Remove(bb);
                    }
                }
            }
            else //敵人小飛機
            {
                if (test < 10) //10%機率出現敵人小飛機
                {
                    CEnemy em = new CEnemy(Properties.Resources.rredplane, test);
                    enemyList.Add(em);
                }

                if (changebullet == 1) //道具掉落
                {
                    if (isItem == false)
                    {
                        item = new CEnemy(Properties.Resources.item, test);
                        isItem = true;
                    }
                }
                airplane.Move();
                for (int i = 0; i < bulletList.Count; i++) //玩家子彈
                {
                    CBullet bm = bulletList[i] as CBullet;
                    bm.Move();
                    if (!bm.isLive) bulletList.Remove(bm);
                }
                for (int i = 0; i < enemyList.Count; i++) //敵人小飛機
                {
                    CEnemy en = enemyList[i] as CEnemy;
                    en.Move();
                    if (en.CheckCrash(airplane.rec)) //撞到玩家飛機
                    {
                        explosion.Play();
                        isGameOver = true;
                        label2.Visible = true;
                        ReturnBtn.Visible = true;
                        ReturnBtn.Enabled = true;
                    }
                    if (en.CheckBoom(bulletList)) //撞到玩家子彈
                    {
                        enemyList.Remove(en);
                        score++;
                        explosion.Play();
                    }
                }

                item.Move2(); //道具移動
                if (item.CheckCrash(airplane.rec) && item.isLive == true) //玩家吃到道具
                {
                    bullettype = 1;
                    item.isLive = false;
                }
                else if (item.isLive == false && bullettype == 0) //道具沒被吃到
                {
                    isItem = false;
                    changebullet = 0;
                }

            }
            this.Invalidate(); //畫圖
        }

        private void StartBtn_Click(object sender, EventArgs e) //開始按鈕
        {
            isGameOver = false;
            isBoss = false;
            gametime = 20;
            stbn = 1;
            timer1.Enabled = true;
            timer2.Enabled = true;
            this.BackgroundImage = Properties.Resources.bg1;
            boss1 = new CBoss(Properties.Resources.bboss1, 10, 200, 5);
            boss2 = new CBoss(Properties.Resources.bboss2, 10, 400, 10);
            boss3 = new CBoss(Properties.Resources.bboss3, 10, 600, 15);
            stage = 1;
            score = 0;
            bullettype = 0;
            item.isLive = false;
            label1.Visible = false;
            StartBtn.Visible = false;
            StartBtn.Enabled = false;
            setboss = false;

            if (comboBox1.Text == "Easy") //設定難易度
                bulletamount = 10;
            else if (comboBox1.Text == "Normal")
                bulletamount = 8;
            else
                bulletamount = 5;
            comboBox1.Visible = false;
            comboBox1.Enabled = false;
        }

        private void ReturnBtn_Click(object sender, EventArgs e) //回主頁面按鈕
        {
            label1.Visible = true;
            StartBtn.Visible = true;
            StartBtn.Enabled = true;
            this.BackgroundImage = Properties.Resources.mainbg;
            ReturnBtn.Visible = false;
            ReturnBtn.Enabled = false;
            label2.Visible = false;
            label3.Visible = false;
            comboBox1.Visible = true;
            comboBox1.Enabled = true;
            enemyList.Clear();
            bulletList.Clear();
            bossbullet.Clear();
            stbn = 0;
        }

        private void timer2_Tick(object sender, EventArgs e) //計算BOSS出場時間和道具出場時間
        {
            gametime--;
            int test = rd.Next(0,100);

            if (test < 6) //道具出場
               changebullet = 1;
            if (gametime <= 0) //BOSS出場
            {
                isBoss = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) //鍵盤輸入
        {
            if (e.KeyCode == Keys.Space) //空白鍵-發射子彈
            {
                if (isGameOver == false)
                {
                    if (bulletList.Count > bulletamount) return;
                    CBullet bullet;
                    if (bullettype == 0)
                        bullet = new CBullet(Properties.Resources.bullet, new Point(0, 0), 1, 0, 10);
                    else
                        bullet = new CBullet(Properties.Resources.bullet2, new Point(0, 0), 1, 0, 20);
                    bullet.pt.X = airplane.pt.X + (airplane.rec.Width / 2) - (bullet.rec.Width / 2);
                    bullet.pt.Y = airplane.pt.Y - bullet.rec.Height;
                    bullet.isLive = true;
                    bulletList.Add(bullet);
                    this.Invalidate();
                    laser.Play();
                }
            }
            if (e.KeyCode == Keys.Left) //左移
            {
                airplane.KeyMove(1);
                this.Invalidate();
            }

            if (e.KeyCode == Keys.Right) //右移
            {
                airplane.KeyMove(2);
                this.Invalidate();
            }
        }
    }
}