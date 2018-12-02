using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBookings
{
    class patient
    {
        string name;
        string ic;
        string phone;
        string birth;
        string gentle;
        string address;
        int areca;
        int smoking;
        int no;
        string []reserveinfo = new string[5];

        public String getname()
        {
            return name;
        }
        public void setname(String s)
        {
            name = s;
            no = 0;
        }
        public String getic()
        {
            return ic;
        }
        public void setic(String s)
        {
            ic = s;
        }
        public String getphone()
        {
            return phone;
        }
        public void setphone(String s)
        {
            phone = s;
        }
        public String getbirth()
        {
            return birth;
        }
        public void setbirth(String s)
        {
            birth = s;
        }
        public String getgentle()
        {
            return gentle;
        }
        public void setgentle(String s)
        {
            gentle = s;
        }
        public String getaddress()
        {
            return address;
        }
        public void setaddress(String s)
        {
            address = s;
        }
        public int getareca()
        {
            return areca;
        }
        public void setareca(int s)
        {
            areca = s;
        }
        public int getsmoking()
        {
            return smoking;
        }
        public void setsmoking(int s)
        {
            smoking = s;
        }
        public int getno()
        {
            return no;
        }
        public void setno(int s)
        {
            no = s;
        }
        public void setreserve(string n,string t,int a,int b)
        {
            reserveinfo[0] = t;
            reserveinfo[1] = n;

            if (a == 1)
                reserveinfo[2] = "星期一";
            else if (a == 2)
                reserveinfo[2] = "星期二";
            else if (a == 3)
                reserveinfo[2] = "星期三";
            else if (a == 4)
                reserveinfo[2] = "星期四";
            else if (a == 5)
                reserveinfo[2] = "星期五";
            else if (a == 6)
                reserveinfo[2] = "星期六";
            else if (a == 7)
                reserveinfo[2] = "星期日";
            else if (a == 0)
                reserveinfo[2] = null;

            if (b == 1)
                reserveinfo[3] = "上午";
            else if (b == 2)
                reserveinfo[3] = "下午";
            else if (b == 3)
                reserveinfo[3] = "夜間";
            else if (b == 0)
                reserveinfo[3] = null;
        }
        public string getreserve(int i)
        {
            return reserveinfo[i];
        }
        public void output()
        {
            Console.WriteLine("看診日期 : " + reserveinfo[2] + " " + reserveinfo[3]);
            Console.WriteLine("看診科別 : " + reserveinfo[0]);
            Console.WriteLine("看診醫師 : " + reserveinfo[1]);
        }
        public void output2()
        {
            Console.WriteLine("挂號成功！");
            Console.WriteLine("");
            Console.WriteLine("姓名/身份證號 : " + name + " / " + ic);
            Console.WriteLine("就診星期/午別 : " + reserveinfo[2] + " " + reserveinfo[3]+"門診");
            Console.WriteLine("看診科別 : " + reserveinfo[0]);
            Console.WriteLine("看診醫師 : " + reserveinfo[1]);
            Console.WriteLine("看診序號 : No.00" + no);
        }
    }
}
