using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class Roadcontrol
    {
        public bool gate;      //статус шлагбаума: 0 - закрыт, 1 - открыт
        public int paycheck;   //статус режима оплаты: 0 - не производится, 1 - ручной режим, 2 - автоматический режим
        public bool light;     //статус светофора: 0 - красный, 1 - зеленый
        public Roadcontrol()
        {
            gate = false;
            light = false;
            paycheck = 0;
        }
        public void openGate()
        {
            gate = true;
        }
        public void closeGate()
        {
            gate = false;
        }
        public void greenLight()
        {
            light = true;
        }
        public void redLight()
        {
            light = false;
        }
        public void noPaycheck()
        {
            paycheck = 0;
        }
        public void manualPaycheck()
        {
            paycheck = 1;
        }
        public void autoPaycheck()
        {
            paycheck = 2;
        }
    }
}
