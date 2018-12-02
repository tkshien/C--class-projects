using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SimplePaint
{
    public partial class SimplePaint : Form
    {
        Color paintcolor, color1, color2;
        Font font;
        bool chooseColor = false;
        bool draw = false;
        int x, y;
        int style = 1;
        int hold = 1;
        Item currItem;
        List<Point> polygonPoints = new List<Point>();
        Rectangle SelectedRect;
        Array obj;
        int temp;

        public SimplePaint()
        {
            InitializeComponent();
        }

        public enum Item
        {
            Pencil, Eraser, Line, Picker, text, fill,
            solidbrush, hatchbrush, lineargradienbrush,
            rectangle, ellipse, polygon, pie,
            bezier, curve,
            rotate, sellect, paste
        }

        private void NEW_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
        }

        private void OPEN_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        private void SAVE_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|All files(*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g.Dispose();

                bmp.Save(saveFileDialog1.FileName);
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            chooseColor = true;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            chooseColor = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (chooseColor)
            {
                Bitmap bmp = (Bitmap)pictureBox2.Image.Clone();
                paintcolor = bmp.GetPixel(e.X, e.Y);
                red.Value = paintcolor.R;
                green.Value = paintcolor.G;
                blue.Value = paintcolor.B;
                alpha.Value = paintcolor.A;
                redlbl.Text = "R: " + paintcolor.R.ToString();
                greenlbl.Text = "G: " + paintcolor.G.ToString();
                bluelbl.Text = "B: " + paintcolor.B.ToString();
                alphalbl.Text = "A: " + paintcolor.A.ToString();
                pictureBox3.BackColor = paintcolor;
            }
        }

        private void alpha_Scroll(object sender, EventArgs e)
        {
            paintcolor = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paintcolor;
            alphalbl.Text = "A: " + paintcolor.A.ToString();
        }

        private void red_Scroll(object sender, EventArgs e)
        {
            paintcolor = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paintcolor;
            redlbl.Text = "R: " + paintcolor.R.ToString();
        }

        private void green_Scroll(object sender, EventArgs e)
        {
            paintcolor = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paintcolor;
            greenlbl.Text = "G: " + paintcolor.G.ToString();
        }

        private void blue_Scroll(object sender, EventArgs e)
        {
            paintcolor = Color.FromArgb(alpha.Value, red.Value, green.Value, blue.Value);
            pictureBox3.BackColor = paintcolor;
            bluelbl.Text = "B: " + paintcolor.B.ToString();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            x = e.X;
            y = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;

            if (currItem == Item.Line)
            {
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawLine(new Pen(paintcolor, Convert.ToInt32(SIZEComboBox1.Text)), new Point(x, y), new Point(e.X, e.Y));
                g.Dispose();
            }

            if (currItem == Item.ellipse)
            {
                Graphics g = pictureBox1.CreateGraphics();

                if (style == 1)
                {
                    g.FillEllipse(new SolidBrush(paintcolor), x, y, e.X - x, e.Y - y);
                }
                else if (style == 2)
                {
                    HatchStyle temp2 = (HatchStyle)obj.GetValue(temp);
                    HatchBrush hb2 = new HatchBrush(temp2, color1, color2);

                    g.FillEllipse(hb2, x, y, e.X - x, e.Y - y);
                }
                else
                {
                    Rectangle rect2 = new Rectangle(0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
                    LinearGradientBrush lgb2 = new LinearGradientBrush(rect2, color1, color2, 1);

                    g.FillEllipse(lgb2, x, y, e.X - x, e.Y - y);
                }

                g.Dispose();
            }

            if (currItem == Item.pie)
            {
                Graphics g = pictureBox1.CreateGraphics();

                if (style == 1)
                {
                    if (e.X < x || e.Y < y)
                        g.FillPie(new SolidBrush(paintcolor), e.X, e.Y, x - e.X, y - e.Y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                    else
                        g.FillPie(new SolidBrush(paintcolor), x, y, e.X - x, e.Y - y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                }
                else if (style == 2)
                {
                    HatchStyle temp2 = (HatchStyle)obj.GetValue(temp);
                    HatchBrush hb2 = new HatchBrush(temp2, color1, color2);

                    if (e.X < x || e.Y < y)
                        g.FillPie(hb2, e.X, e.Y, x - e.X, y - e.Y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                    else
                        g.FillPie(hb2, x, y, e.X - x, e.Y - y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                }
                else
                {
                    Rectangle rect2 = new Rectangle(0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
                    LinearGradientBrush lgb2 = new LinearGradientBrush(rect2, color1, color2, 1);

                    if (e.X < x || e.Y < y)
                        g.FillPie(lgb2, e.X, e.Y, x - e.X, y - e.Y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                    else
                        g.FillPie(lgb2, x, y, e.X - x, e.Y - y, Convert.ToInt32(SA1tb.Text), Convert.ToInt32(SA2tb.Text));
                }

                g.Dispose();
            }

            if (currItem == Item.sellect)
            {
                Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g1 = Graphics.FromImage(bmp1);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g1.DrawImage(bmp1, 0, 0);
                pictureBox1.Image = bmp1;
                g1.Dispose();

                Rectangle src_rect = new Rectangle(x, y, e.X - x, e.Y - y);
                Rectangle dest_rect = new Rectangle(0, 0, src_rect.Width, src_rect.Height);
                SelectedRect = src_rect;
                Image img = pictureBox1.Image;
                Bitmap bm = new Bitmap(src_rect.Width, src_rect.Height);

                Graphics gfx = Graphics.FromImage(bm);
                gfx.DrawImage(img, dest_rect, src_rect, GraphicsUnit.Pixel);

                Clipboard.SetImage(bm);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics g = pictureBox1.CreateGraphics();
                switch (currItem)
                {
                    case Item.Pencil:
                        g.DrawLine(new Pen(paintcolor, Convert.ToInt32(SIZEComboBox1.Text)), x, y, e.X, e.Y);
                        x = e.X;
                        y = e.Y;
                        break;

                    case Item.Eraser:
                        g.DrawLine(new Pen(Color.White, Convert.ToInt32(SIZEComboBox1.Text)), x, y, e.X, e.Y);
                        x = e.X;
                        y = e.Y;
                        break;

                    case Item.solidbrush:
                        SolidBrush sb = new SolidBrush(paintcolor);
                        g.FillEllipse(sb, x, y, Convert.ToInt32(SIZEComboBox1.Text), Convert.ToInt32(SIZEComboBox1.Text));
                        x = e.X;
                        y = e.Y;
                        break;

                    case Item.hatchbrush:
                        HatchStyle temp2 = (HatchStyle)obj.GetValue(temp);
                        HatchBrush hb = new HatchBrush(temp2, color1, color2);
                        g.FillEllipse(hb, x, y, Convert.ToInt32(SIZEComboBox1.Text), Convert.ToInt32(SIZEComboBox1.Text));
                        x = e.X;
                        y = e.Y;
                        break;

                    case Item.lineargradienbrush:
                        Rectangle rect = new Rectangle(0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
                        LinearGradientBrush lgb = new LinearGradientBrush(rect, color1, color2, 1);                       
                        g.FillEllipse(lgb, x, y, Convert.ToInt32(SIZEComboBox1.Text), Convert.ToInt32(SIZEComboBox1.Text));
                        x = e.X;
                        y = e.Y;
                        break;

                    case Item.rectangle:
                        if (style == 1)
                        {
                            if (e.X < x || e.Y < y)
                                g.FillRectangle(new SolidBrush(paintcolor), e.X, e.Y, x - e.X, y - e.Y);
                            else
                                g.FillRectangle(new SolidBrush(paintcolor), x, y, e.X - x, e.Y - y);
                        }
                        else if (style == 2)
                        {
                            HatchStyle temp3 = (HatchStyle)obj.GetValue(temp);

                            HatchBrush hb2 = new HatchBrush(temp3, color1, color2);
                            if (e.X < x || e.Y < y)
                                g.FillRectangle(hb2, e.X, e.Y, x - e.X, y - e.Y);
                            else
                                g.FillRectangle(hb2, x, y, e.X - x, e.Y - y);
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
                            LinearGradientBrush lgb2 = new LinearGradientBrush(rect2, color1, color2, 1);
                            if (e.X < x || e.Y < y)
                                g.FillRectangle(lgb2, e.X, e.Y, x - e.X, y - e.Y);
                            else
                                g.FillRectangle(lgb2, x, y, e.X - x, e.Y - y);
                        }
                        break;
                }
                g.Dispose();
            }
        }

        private void Pen_Click(object sender, EventArgs e)
        {
            currItem = Item.Pencil;
        }

        private void Eraser_Click(object sender, EventArgs e)
        {
            currItem = Item.Eraser;
        }

        private void SimplePaint_Load(object sender, EventArgs e)
        {
            for (int i = 2; i <= 20; i += 2)
                SIZEComboBox1.Items.Add(i);
            SIZEComboBox1.Text = "2";

            panel2.Visible = false;
            panel3.Visible = false;

            obj = Enum.GetValues(typeof(HatchStyle));

            for (int i = 0; i < obj.Length; i++)
            {
                HatchcomboBox1.Items.Add(obj.GetValue(i));
                HBcomboBox1.Items.Add(obj.GetValue(i));
            }

            HatchcomboBox1.Visible = false;
        }

        private void line_Click(object sender, EventArgs e)
        {
            currItem = Item.Line;
        }

        private void Picker_Click(object sender, EventArgs e)
        {
            currItem = Item.Picker;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currItem == Item.Picker)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g.Dispose();
                paintcolor = bmp.GetPixel(e.X, e.Y);
                pictureBox3.BackColor = paintcolor;
                red.Value = paintcolor.R;
                green.Value = paintcolor.G;
                blue.Value = paintcolor.B;
                alpha.Value = paintcolor.A;
                redlbl.Text = "R: " + paintcolor.R.ToString();
                greenlbl.Text = "G: " + paintcolor.G.ToString();
                bluelbl.Text = "B: " + paintcolor.B.ToString();
                alphalbl.Text = "A: " + paintcolor.A.ToString();
                bmp.Dispose();
                g.Dispose();
            }

            if (currItem == Item.text && toolStripTextBox1.Text != "")
            {
                Graphics g = pictureBox1.CreateGraphics();

                g.DrawString(toolStripTextBox1.Text, font, new SolidBrush(paintcolor), e.X, e.Y);
                g.Dispose();
            }

            if (currItem == Item.polygon)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        polygonPoints.Add(new Point(e.X, e.Y));
                        break;

                    case MouseButtons.Right:
                        Graphics g = pictureBox1.CreateGraphics();
                        polygonPoints.Add(new Point(e.X, e.Y));
                        Point[] point = polygonPoints.ToArray();
                        g.DrawPolygon(new Pen(paintcolor, Convert.ToInt32(SIZEComboBox1.Text)), point);
                        polygonPoints.Clear();
                        g.Dispose();
                        break;
                }
            }

            if (currItem == Item.bezier)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        polygonPoints.Add(new Point(e.X, e.Y));
                        break;

                    case MouseButtons.Right:
                        Graphics g = pictureBox1.CreateGraphics();
                        polygonPoints.Add(new Point(e.X, e.Y));
                        Point[] point = polygonPoints.ToArray();
                        g.DrawBezier(new Pen(paintcolor, Convert.ToInt32(SIZEComboBox1.Text)), point[0], point[1], point[2], point[3]);
                        polygonPoints.Clear();
                        g.Dispose();
                        break;
                }
            }

            if (currItem == Item.curve)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        polygonPoints.Add(new Point(e.X, e.Y));
                        break;

                    case MouseButtons.Right:
                        Graphics g = pictureBox1.CreateGraphics();
                        polygonPoints.Add(new Point(e.X, e.Y));
                        Point[] point = polygonPoints.ToArray();
                        g.DrawCurve(new Pen(paintcolor, Convert.ToInt32(SIZEComboBox1.Text)), point);
                        polygonPoints.Clear();
                        g.Dispose();
                        break;
                }
            }

            if (currItem == Item.paste)
            {
                Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g1 = Graphics.FromImage(bmp1);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g1.DrawImage(bmp1, 0, 0);
                pictureBox1.Image = bmp1;
                g1.Dispose();

                SelectedRect = new Rectangle(x, y, e.X - x, e.Y - y);

                Image clipboard_image = Clipboard.GetImage();


                int cx = SelectedRect.X + (SelectedRect.Width - clipboard_image.Width) / 2;

                int cy = SelectedRect.Y + (SelectedRect.Height - clipboard_image.Height) / 2;

                Rectangle dest_rect = new Rectangle(cx, cy, clipboard_image.Width, clipboard_image.Height);

                Image img = pictureBox1.Image;
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                Graphics gr = Graphics.FromImage(bmp);

                gr.DrawImage(clipboard_image, dest_rect);

                clipboard_image = bmp;

                pictureBox1.BackgroundImage = bmp1;
                pictureBox1.Image = clipboard_image;

            }

            if (currItem == Item.fill)
            {
                Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g1 = Graphics.FromImage(bmp1);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g1.DrawImage(bmp1, 0, 0);
                pictureBox1.Image = bmp1;
                g1.Dispose();

                Stack<Point> pixels = new Stack<Point>();
                Color targetColor = bmp1.GetPixel(e.X, e.Y);
                Point pt = new Point(e.X,e.Y);
                pixels.Push(pt);

                while (pixels.Count > 0)
                {
                    Point a = pixels.Pop();
                    if (a.X < bmp1.Width && a.X > 0 &&a.Y < bmp1.Height && a.Y > 0)              
                    {

                        if (bmp1.GetPixel(a.X, a.Y) == targetColor)
                        {
                            bmp1.SetPixel(a.X, a.Y, paintcolor);
                            pixels.Push(new Point(a.X - 1, a.Y));
                            pixels.Push(new Point(a.X + 1, a.Y));
                            pixels.Push(new Point(a.X, a.Y - 1));
                            pixels.Push(new Point(a.X, a.Y + 1));
                        }
                    }
                }
                pictureBox1.Image = bmp1;
           }
        }

        private void TEXT_Click(object sender, EventArgs e)
        {
            currItem = Item.text;
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                font = fontDialog1.Font;
                toolStripTextBox2.Text = font.Name;
            }
        }

        private void solidBrushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.solidbrush;
        }

        private void hatchBrushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.hatchbrush;
            panel3.Visible = true;
        }

        private void linearGradienBrushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.lineargradienbrush;
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.rectangle;
            panel2.Visible = true;
        }

        private void eclipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.ellipse;
            panel2.Visible = true;
        }

        private void polygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.polygon;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            color1 = paintcolor;
            pictureBox4.BackColor = paintcolor;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            color2 = paintcolor;
            pictureBox5.BackColor = paintcolor;
        }

        private void pieToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void drawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currItem = Item.pie;
            panel2.Visible = true;
        }

        private void Bezier_Click(object sender, EventArgs e)
        {
            currItem = Item.bezier;
        }

        private void Curve_Click(object sender, EventArgs e)
        {
            currItem = Item.curve;
        }

        private void Rotate_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
            g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
            g1.DrawImage(bmp1, 0, 0);
            pictureBox1.Image = bmp1;
            g1.Dispose();


            Image img = pictureBox1.Image;
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            Graphics gfx = Graphics.FromImage(bmp);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            gfx.RotateTransform(90);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(img, new Point(0, 0));

            gfx.Dispose();

            img = bmp;

            pictureBox1.Image = img;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                style = 1;
                panel2.Visible = false;
            }
            if (radioButton2.Checked == true)
            {
                style = 2;

                if (hold == 1)
                {
                    panel2.Visible = true;
                    HatchcomboBox1.Visible = true;
              
                    hold = 2;
                }
                else if (hold == 2)
                {
                    for (int i = 0; i < obj.Length; i++)
                    {
                        if (Convert.ToString(obj.GetValue(i)).Equals(HatchcomboBox1.Text))
                        {
                            temp = i;
                            break;
                        }
                    }
                    panel2.Visible = false;
                    HatchcomboBox1.Visible = false;
                    hold = 1;
                }
            }
            if (radioButton3.Checked == true)
            {
                style = 3;
                panel2.Visible = false;
            }
        }

        private void Sellect_Click(object sender, EventArgs e)
        {
            currItem = Item.sellect;
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            currItem = Item.paste;
        }

        private void Cut_Click(object sender, EventArgs e)
        {
                Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g1 = Graphics.FromImage(bmp1);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g1.DrawImage(bmp1, 0, 0);
                pictureBox1.Image = bmp1;
                g1.Dispose();

                Image img = pictureBox1.Image;
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                Graphics gr = Graphics.FromImage(bmp);

                gr.FillRectangle(new SolidBrush(Color.White), SelectedRect);

                img = bmp;
                pictureBox1.BackgroundImage = bmp1;
                pictureBox1.Image = img;   
        }

        private void SelectRotate_Click(object sender, EventArgs e)
        {
            Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
            g1.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
            g1.DrawImage(bmp1, 0, 0);
            pictureBox1.Image = bmp1;
            g1.Dispose();

            Image clipboard_image = Clipboard.GetImage();

            Bitmap bmp = new Bitmap(clipboard_image.Width, clipboard_image.Height);

            Graphics gfx = Graphics.FromImage(bmp);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            gfx.RotateTransform(90);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            gfx.DrawImage(clipboard_image, new Point(0, 0));

            gfx.Dispose();

            clipboard_image = bmp;

            int cx = SelectedRect.X + (SelectedRect.Width - clipboard_image.Width) / 2;

            int cy = SelectedRect.Y + (SelectedRect.Height - clipboard_image.Height) / 2;

            Rectangle dest_rect = new Rectangle(cx, cy, clipboard_image.Width, clipboard_image.Height);

            Image img = pictureBox1.Image;
            Bitmap bmp2 = new Bitmap(img.Width, img.Height);

            Graphics gr = Graphics.FromImage(bmp2);

            gr.DrawImage(clipboard_image, dest_rect);

            clipboard_image = bmp2;

            pictureBox1.BackgroundImage = bmp1;
            pictureBox1.Image = clipboard_image;
        }

        private void SimplePaint_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("是否要存檔？", "Simple Paint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                saveFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|All files(*.*)|*.*";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                    g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                    g.Dispose();

                    bmp.Save(saveFileDialog1.FileName);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                if (Convert.ToString(obj.GetValue(i)).Equals(HBcomboBox1.Text))
                {
                    temp = i;
                    break;
                }
            }
            panel3.Visible = false;
        }

        private void Fill_Click(object sender, EventArgs e)
        {
            currItem = Item.fill;
        }
    }
}
