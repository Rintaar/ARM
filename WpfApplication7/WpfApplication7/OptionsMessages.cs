using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class OptionsMessages
    {
        public int[,] A = new int[,] { {106, 112, 123, 135} ,
                                             {155, 204, 215, 233} ,
                                             {437, 447, 458, 511} ,
                                             {530, 539, 551, 608} ,
                                             {735, 737, 748, 758 } };
        public int[,] B = new int[,] { {758, 802, 806, 815} ,
                                             {920, 930, 947, 1001} ,
                                             {1357,1405, 1421, 1430} ,
                                              };
        public int[,] C = new int[,] { { 106, 112, 123, 135 } };
        public int[,] D = new int[,] { { 106, 112, 123, 135 } };
        public int[,] E = new int[,] { {818, 818, 818, 825} ,
                                             {1137, 1142, 1145, 1149} ,
                                             {2411, 2412, 2420, 2424}
                                            };
        public OptionsMessages()
        { }        
        public string printcash(int moneyin, int moneyreal)
        {        
            string s = "Получено: " + MoneyIntToStr(moneyreal) + "\nОплата: " + MoneyIntToStr(moneyin) + "\nСдача: " + MoneyIntToStr((moneyreal - moneyin));
            return s;
        }
        public string printbsccash(string moneynum, int pay, int moneyout)
        {
            string s = "Карта номер: " + moneynum + "\nОплата: " + MoneyIntToStr(pay) + "\nОстаток на карте: " + MoneyIntToStr(moneyout - pay);
            return s;
        }
        //переводит деньги из str в int
        public int MoneyStrToInt(string s)
        {
            int value;
            if (s.Contains(".") == true) s = s.Replace(".", "");
            else s += "00";
            return value = Convert.ToInt32(s);
        }
        //переводит деньги из int в str
        public string MoneyIntToStr(int money)
        {
            string s;
            string s1 = Convert.ToString(money % 100);
            if (s1.Length == 1) s1 += "0";
            return s = Convert.ToString(money / 100) + "." + s1;
        }
        public int timers(int cls, int pos, int num)
        {
            int x = 0;
            switch (cls)
            {
                case 1:
                    {
                        x = A[num, pos];
                    }
                    break;
                case 2:
                    {
                        x = B[num, pos];
                    }
                    break;
                case 3:
                    {
                        x = C[num, pos];
                    }
                    break;
                case 4:
                    {
                        x = D[num, pos];
                    }
                    break;
            }
            return x;
           
        }
    }
}
