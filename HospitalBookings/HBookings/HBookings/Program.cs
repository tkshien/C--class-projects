using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HBookings
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice = 0, choice1 = 0;
            int dlength = 0, slength = 0, mlength = 0, plength = 0;

            doctor[] dentist = new doctor[10];
            doctor[] surgical = new doctor[10];
            doctor[] medical = new doctor[10];
            patient[] pat = new patient[100];

            input(dentist, surgical, medical, ref dlength, ref slength, ref mlength);
            //output(dentist, surgical, medical, ref dlength, ref slength, ref mlength);

            while (choice != -1)
            {
                Console.WriteLine("===========================================");
                Console.Write("1)網路挂號 2)查詢/取消挂號 -1)寫檔結束 :");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("");
                        Console.WriteLine("請選擇科別：");
                        Console.WriteLine("");
                        Console.Write("1)牙科門診 2）一般外科 3）一般內科：");
                        choice1 = Convert.ToInt32(Console.ReadLine());

                        if (choice1 == 1)
                        {
                            reserve(dentist, pat, dlength, ref plength);
                        }
                        else if (choice1 == 2)
                        {
                            reserve(surgical, pat, slength, ref plength);
                        }
                        else if (choice1 == 3)
                        {
                            reserve(medical, pat, mlength, ref plength);
                        }
                        break;
                    case 2:
                        search(dentist, surgical, medical, pat, dlength, slength, mlength, plength);
                        break;
                    case -1:
                        writefile(pat, plength);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void input(doctor[] dentist, doctor[] surgical, doctor[] medical, ref int dlength, ref int slength, ref int mlength)
        {
            StreamReader sr = new StreamReader(@"C:\Users\acer\Desktop\doctor.txt");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                String[] elements = line.Split(' ');

                doctor d = new doctor(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7]);

                if (elements[0].Equals("dentist"))
                {
                    dentist[dlength++] = d;
                }
                else if (elements[0].Equals("surgical"))
                {
                    surgical[slength++] = d;
                }
                else if (elements[0].Equals("medical"))
                {
                    medical[mlength++] = d;
                }
            }
            sr.Close();
        }

        public static void output(doctor[] dentist, doctor[] surgical, doctor[] medical, ref int dlength, ref int slength, ref int mlength)
        {
            string[,] timetable = new string[3, 7];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 7; j++)
                    timetable[i, j] = null;

            Console.WriteLine("Dentist:");

            for (int i = 0; i < dlength; i++)
            {
                Console.WriteLine(dentist[i].getname());
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (dentist[i].getworkday(j, k) == 1)
                        {
                            timetable[j, k] = dentist[i].getname();
                        }
                    }
                }
            }
            Console.WriteLine("星期一\t星期二\t星期三\t星期四\t星期五\t星期六\t星期日");

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Console.Write(timetable[i, j] + "\t");
                    timetable[i, j] = null;
                }
                Console.WriteLine("");
            }

            Console.WriteLine("Surgical:");
            for (int i = 0; i < slength; i++)
            {
                Console.WriteLine(surgical[i].getname());
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (surgical[i].getworkday(j, k) == 1)
                        {
                            timetable[j, k] = surgical[i].getname();
                        }
                    }
                }
            }

            Console.WriteLine("星期一\t星期二\t星期三\t星期四\t星期五\t星期六\t星期日");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Console.Write(timetable[i, j] + "\t");
                    timetable[i, j] = null;
                }
                Console.WriteLine("");
            }


            Console.WriteLine("Medical:");
            for (int i = 0; i < mlength; i++)
            {
                Console.WriteLine(medical[i].getname());
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (medical[i].getworkday(j, k) == 1)
                        {
                            timetable[j, k] = medical[i].getname();
                        }
                    }
                }
            }
            Console.WriteLine("星期一\t星期二\t星期三\t星期四\t星期五\t星期六\t星期日");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Console.Write(timetable[i, j] + "\t");
                    timetable[i, j] = null;
                }
                Console.WriteLine("");
            }
        }

        public static void reserve(doctor[] doc, patient[] pat, int dlength, ref int plength)
        {
            string[,] timetable = new string[3, 7];
            int choice, choice2;
            string temp;
            int hold = 0;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 7; j++)
                    timetable[i, j] = null;

            for (int i = 0; i < dlength; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        if (doc[i].getworkday(j, k) == 1)
                        {
                            if (doc[i].getreservation(j, k) < 5)
                                timetable[j, k] = doc[i].getname();
                            else
                                timetable[j, k] = doc[i].getname() + "滿";
                        }
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("\t星期一\t星期二\t星期三\t星期四\t星期五\t星期六\t星期日");

            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    Console.Write("上午\t");
                else if (i == 1)
                    Console.Write("下午\t");
                else if (i == 2)
                    Console.Write("夜間\t");
                for (int j = 0; j < 7; j++)
                {
                    Console.Write(timetable[i, j] + "\t");
                    timetable[i, j] = null;
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.WriteLine("選擇挂哪一天：");
            Console.WriteLine("");
            Console.Write("1)星期一 2)星期二 3)星期三 4)星期四 5)星期五 6)星期六 7)星期日 ：");
            choice = Convert.ToInt32(Console.ReadLine());

            Console.Write("1)上午 2)下午 3)夜間：");
            choice2 = Convert.ToInt32(Console.ReadLine());

            for (int z = 0; z < dlength; z++)
            {
                if (doc[z].getworkday(choice2 - 1, choice - 1) == 1 && doc[z].getreservation(choice2 - 1, choice - 1) < 5)
                {
                    Console.WriteLine("請輸入身份證：");
                    temp = Console.ReadLine();

                    for (int i = 0; i < plength; i++)
                    {
                        if (pat[i].getic().Equals(temp))
                        {
                            hold = 1;
                        }
                    }

                    if (hold == 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("初診");

                        patient p = new patient();

                        p.setreserve(doc[z].getname(), doc[z].gettype(), choice, choice2);
                        Console.WriteLine("");
                        p.output();
                        Console.WriteLine("");
                        doc[z].setreservation(choice2 - 1, choice - 1);

                        Console.Write("輸入名字：");
                        p.setname(Console.ReadLine());
                        Console.Write("輸入身份證：");
                        p.setic(Console.ReadLine());
                        Console.Write("輸入手機號碼：");
                        p.setphone(Console.ReadLine());
                        Console.Write("輸入生日：");
                        p.setbirth(Console.ReadLine());
                        Console.Write("輸入性別：");
                        p.setgentle(Console.ReadLine());
                        Console.Write("輸入地址：");
                        p.setaddress(Console.ReadLine());
                        Console.Write("嚼檳榔 1）是 2）否：");
                        p.setareca(Convert.ToInt32(Console.ReadLine()));
                        Console.Write("吸菸 ：1）是 2）否：");
                        p.setsmoking(Convert.ToInt32(Console.ReadLine()));
                        p.setno(doc[z].getreservation(choice2 - 1, choice - 1));
                        pat[plength++] = p;
                        Console.WriteLine("");
                        p.output2();
                        Console.WriteLine("");
                        return;
                    }
                    else if (hold == 1)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("復診");

                        for (int i = 0; i < plength; i++)
                        {
                            if (pat[i].getic().Equals(temp))
                            {
                                for (int h = 0; h < dlength; h++)
                                {
                                    if (doc[h].getworkday(choice2 - 1, choice - 1) == 1 && doc[h].getreservation(choice2 - 1, choice - 1) < 5)
                                    {
                                        pat[i].setreserve(doc[h].getname(), doc[h].gettype(), choice, choice2);
                                        Console.WriteLine("");
                                        pat[i].output();
                                        Console.WriteLine("");
                                        doc[h].setreservation(choice2 - 1, choice - 1);
                                        pat[i].setno(doc[h].getreservation(choice2 - 1, choice - 1));
                                        Console.WriteLine("");
                                        pat[i].output2();
                                        Console.WriteLine("");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("輸入錯誤！");
            Console.WriteLine("");
        }

        public static void search(doctor[] dentist, doctor[] surgical, doctor[] medical, patient[] pat, int dlength, int slength, int mlength, int plength)
        {
            string temp;
            string temp2;
            int choice;
            int a = 0;
            int b = 0;

            Console.WriteLine("");
            Console.Write("輸入身份證號：");
            temp = Console.ReadLine();
            Console.Write("輸入出生日期：");
            temp2 = Console.ReadLine();

            for (int i = 0; i < plength; i++)
            {
                if (pat[i].getic().Equals(temp) && pat[i].getbirth().Equals(temp2) && pat[i].getno() != 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("身份證：" + temp + "\t" + "姓名：" + pat[i].getname());
                    Console.WriteLine("");
                    Console.WriteLine("日期時段\t科別\t\t看診序號\t醫生");
                    Console.WriteLine(pat[i].getreserve(2) + pat[i].getreserve(3) + "\t" + pat[i].getreserve(0) + "\t\t" + "00" + pat[i].getno() + "\t\t" + pat[i].getreserve(1));
                    Console.WriteLine("");
                    Console.Write("取消挂號 1)是 2）否：");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 1)
                    {
                        if (pat[i].getreserve(2).Equals("星期一"))
                            a = 0;
                        else if (pat[i].getreserve(2).Equals("星期二"))
                            a = 1;
                        else if (pat[i].getreserve(2).Equals("星期三"))
                            a = 2;
                        else if (pat[i].getreserve(2).Equals("星期四"))
                            a = 3;
                        else if (pat[i].getreserve(2).Equals("星期五"))
                            a = 4;
                        else if (pat[i].getreserve(2).Equals("星期六"))
                            a = 5;
                        else if (pat[i].getreserve(2).Equals("星期日"))
                            a = 6;

                        if (pat[i].getreserve(3).Equals("上午"))
                            b = 0;
                        else if (pat[i].getreserve(3).Equals("下午"))
                            b = 1;
                        else if (pat[i].getreserve(3).Equals("夜間"))
                            b = 2;

                        if (pat[i].getreserve(0).Equals("dentist"))
                        {
                            for (int x = 0; x < dlength; x++)
                            {
                                if (dentist[x].getname().Equals(pat[i].getreserve(1)))
                                {
                                    dentist[x].delreservation(b, a);
                                }
                            }
                        }
                        else if (pat[i].getreserve(0).Equals("surgical"))
                        {
                            for (int x = 0; x < slength; x++)
                            {
                                if (surgical[x].getname().Equals(pat[i].getreserve(1)))
                                {
                                    surgical[x].delreservation(b, a);
                                }
                            }
                        }
                        else if (pat[i].getreserve(0).Equals("medical"))
                        {
                            for (int x = 0; x < mlength; x++)
                            {
                                if (medical[x].getname().Equals(pat[i].getreserve(1)))
                                {
                                    medical[x].delreservation(b, a);
                                }
                            }
                        }
                        pat[i].setno(0);
                        pat[i].setreserve(null, null, 0, 0);
                        Console.WriteLine("取消成功");
                        Console.WriteLine("");
                    }
                    return;
                }
            }
            Console.Write("");
            Console.WriteLine("查無此資料");
        }

        public static void writefile(patient[] pat, int plength)
        {
            StreamWriter sw = new StreamWriter(@"C:\Users\acer\Desktop\patient.txt");
            sw.WriteLine("");

            for (int i = 0; i < plength; i++)
            {
                sw.WriteLine("姓名/身份證號 : " + pat[i].getname() + " / " + pat[i].getic());
                sw.WriteLine("就診星期/午別 : " + pat[i].getreserve(2) + " " + pat[i].getreserve(3) + "門診");
                sw.WriteLine("看診科別 : " + pat[i].getreserve(0));
                sw.WriteLine("看診醫師 : " + pat[i].getreserve(1));
                sw.WriteLine("看診序號 : No.00" + pat[i].getno());
                sw.WriteLine("");
                sw.WriteLine("===========================================");
                sw.WriteLine("");
            }
            sw.Close();
        }
    }
}
