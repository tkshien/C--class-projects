using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCalculator
{
    public partial class Form1 : Form
    {
        double[] number = new double[10];
        int[] operand = new int[10];
        double ans = 0;
        int count1 = 0, count2 = 0, pre = 0, start = 1;
        String equation = null, temp;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Annie");
            comboBox1.Text = "BOBO";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "7";
            start = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "8";
            start = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "9";
            start = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "4";
            start = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "5";
            start = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "6";
            start = 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "1";
            start = 0;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "2";
            start = 0;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "3";
            start = 0;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + "0";
            start = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            ans = 0;
            count1 = count2 = 0;
            equation = null;
            start = 1;
            pre = 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pre == 1)
            {
                equation = equation + textBox1.Text + " + ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                number[count1 - 1] *= -1;
                operand[count2++] = 1;
                textBox1.Text = "";
                start = 1;
                pre = 0;
            }
            else
            {
                equation = equation + textBox1.Text + " + ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                operand[count2++] = 1;
                textBox1.Text = "";
                start = 1;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (start == 1)
            {
                equation = equation + textBox1.Text + " - ";
                pre = 1;
            }
            else if (pre == 1)
            {
                equation = equation + textBox1.Text + " - ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                number[count1 - 1] *= -1;
                operand[count2++] = 2;
                textBox1.Text = "";
                start = 1;
                pre = 0;
            }
            else
            {
                equation = equation + textBox1.Text + " - ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                operand[count2++] = 2;
                textBox1.Text = "";
                start = 1;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (pre == 1)
            {
                equation = equation + textBox1.Text + " x ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                number[count1 - 1] *= -1;
                operand[count2++] = 3;
                textBox1.Text = "";
                pre = 0;
                start = 1;
            }
            else
            {
                equation = equation + textBox1.Text + " x ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                operand[count2++] = 3;
                textBox1.Text = "";
                start = 1;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (pre == 1)
            {
                equation = equation + textBox1.Text + " / ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                number[count1 - 1] *= -1;
                operand[count2++] = 4;
                textBox1.Text = "";
                pre = 0;
                start = 1;
            }
            else
            {
                equation = equation + textBox1.Text + " / ";
                number[count1++] = Convert.ToDouble(textBox1.Text);
                operand[count2++] = 4;
                textBox1.Text = "";
                start = 1;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (pre == 1)
            {
                number[count1++] = Convert.ToDouble(textBox1.Text);
                number[count1 - 1] *= -1;
            }
            else
            {
                number[count1++] = Convert.ToDouble(textBox1.Text);
            }
            if (count2 >= count1)
            {
                textBox1.Text = "ERROR";
            }
            else
            {
                ans = number[0];
                for (int i = 0; i < count2; i++)
                {
                    if (operand[i] == 1)
                    {
                        ans += number[i + 1];
                    }
                    if (operand[i] == 2)
                    {
                        ans -= number[i + 1];
                    }
                    if (operand[i] == 3)
                    {
                        ans *= number[i + 1];
                    }
                    if (operand[i] == 4)
                    {
                        ans /= number[i + 1];
                    }
                }
                temp = Convert.ToString(ans);
                equation = equation + textBox1.Text + " = " + temp;
                textBox1.Text = equation;
                textBox2.Text = "Created by TKS";
            }
        }
    }
}
