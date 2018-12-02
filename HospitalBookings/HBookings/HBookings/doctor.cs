using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBookings
{
    class doctor
    {
        string name;
        string type;
        int[,] workday = new int[,] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } };
        int[,] reservation = new int[,] { { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 } };


        public doctor(string t,string i, string mor, string a, string after, string b, string night, string c)
        {
            int temp;

            name = i;
            type = t;

            temp = Convert.ToInt32(a);

            while (temp % 10 != 0)
            {
                workday[0, (temp % 10) - 1] = 1;
                temp /= 10;
            }


            temp = Convert.ToInt32(b);

            while (temp % 10 != 0)
            {
                workday[1, (temp % 10) - 1] = 1;
                temp /= 10;
            }


            temp = Convert.ToInt32(c);

            while (temp % 10 != 0)
            {
                workday[2, (temp % 10) - 1] = 1;
                temp /= 10;
            }

        }
        public String getname()
        {
            return name;
        }
        public String gettype()
        {
            return type;
        }
        public void setname(String s)
        {
            name = s;
        }
        public int getworkday(int i, int j)
        {
            return workday[i, j];
        }
        public int getreservation(int i, int j)
        {
            return reservation[i, j];
        }
        public void setreservation(int i,int j)
        {
            (reservation[i, j])++;
        }
        public void delreservation(int i, int j)
        {
            (reservation[i, j])--;
        }
    }
}
