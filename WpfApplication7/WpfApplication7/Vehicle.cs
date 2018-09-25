using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class Vehicle
    {
        public int vehClass;
        public int num;
        
        public Vehicle()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int x;
            vehClass = r.Next(4) + 1;
            switch(vehClass)
            {
                case 1:
                    {
                        num = r.Next(5);
                    }
                    break;
                case 2:
                    num = r.Next(3);
                    break;
                case 3:
                    num = r.Next(1); 
                    break;
                case 4:
                    num = r.Next(1); 
                    break;
                case 5:
                    num = r.Next(3);
                    break;
            }
        }
        public void setClass(int a)
        {
            vehClass = a;
        }
        public int getClass()
        {
            return vehClass;
        }
    }
}
