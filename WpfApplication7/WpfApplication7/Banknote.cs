using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class Banknote
    {
        public int nominal; //значение купюры. В данном случае даже не купюры, а денежной суммы, что предоставляет ВОДИТЕЛЬ за оплату.
        public bool IsFake; //свойство денег "ФАЛЬШИВЫЕ". Для купюр номиналом более суммы n, получаемой от водителя. True - купюра подлинная, False - подделка
        Random rand = new Random();
        Random trfls = new Random();
        public Banknote(int val, int nom)
        {
            //val - минимальная сумма на платеж
            //nom - сумма после  которой требуется проверка купюр на подлинность  
            int x = val;
            int value = 0;
            int[] array = new int[10] { 50, 100, 200, 500, 1000, 5000, 10000, 50000, 100000, 500000 };
            Random rnd = new Random(DateTime.Now.Millisecond);
            int rcase = rnd.Next(10);
            if (rcase == 9)
            { value = x - array[0] * rnd.Next(2) - array[1] * rnd.Next(2) - array[3] * rnd.Next(4) - array[3] * rnd.Next(1); }
            else if (rcase > 4)
            {
                value = x;
            }
            else
            {
                int switching = rnd.Next(4);
                int x1 = 0, x2 = 0, x3 = 0;
                while (x > array[x1]) x1++;
                x2 = array[x1 - 1];
                while (x3 < x)
                {
                    x3 += array[rnd.Next(4, x1 - 1)];
                }
                switch (switching)
                {
                    case 0:
                        value = array[rnd.Next(x1, 9)];
                        break;
                    case 1:
                        value = array[rnd.Next(x1, 9)] + x % x2;
                        break;
                    case 2:
                        value = array[rnd.Next(x1, 9)] + x % 1000;
                        break;
                    case 3:
                        value = x3;
                        break;
                    case 4:
                        value = x3 + x % x2;
                        break;
                }
            }
            nominal = value;
            if (value < nom) IsFake = true;
            else
            {
                int x1 = trfls.Next(9);
                if (x1 >= 8) IsFake = false; // т.о. шанс выпадения фальшивой купюры - 20%
                else IsFake = true;
            }         
        }
                
    }
}
