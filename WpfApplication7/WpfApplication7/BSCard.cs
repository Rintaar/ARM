using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class BSCard
    {
        public int value;
        public DateTime date;
        public string number;
        public DateTime dateOut;
        public bool trigger;
        Random r = new Random(DateTime.Now.Millisecond);
        public BSCard()
        {
            number = Rnumber();
            value = r.Next(5000)*100;
            DateTime now = DateTime.Now;            
            date = GetRandomDate(new DateTime((now.Year - 5), 12, 31),now );
            dateOut = GetRandomDate(now, new DateTime((now.Year + 5), 12, 31));
            if (r.Next(10) >= 7) trigger = false;
            else trigger = true;
        }
        public string Rnumber()
        {                      
           
            return (r.Next(9000)+1000).ToString()+ "9" + "000000" + (r.Next(900)+100).ToString();
        }
        
        public DateTime GetRandomDate(DateTime from, DateTime to, Random random = null)
        {
            if (from >= to)
            {
                throw new Exception("Параметр \"from\" должен быть меньше параметра \"to\"!");
            }
            if (random == null)
            {
                random = new Random();
            }
            int daysDiff = (to - from).Days;
            return from.AddDays(random.Next(daysDiff));
        }
    }
}
