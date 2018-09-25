using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class MoneyBox
    {
        int money_real; //сумма находящихся в ящике денег по факту
        int money_visible; //сумма находящихся в ящике денег в теории
        public MoneyBox()
        {
            money_real = money_visible = 0;
        }
        public void addMoney(Banknote money) //внесение купюр в ящик.
        {
            if (money.IsFake == true)
            {
                money_real += money.nominal;
            }
            

        }
        public void addCompMoney(int money) //внесение денег через АРМ, возврат не отданных водителю денег в ящик.
        {
            money_real += money;
        }
        public void addVizMoney(int money) //внесение денег через АРМ, возврат не отданных водителю денег в ящик.
        {
            money_visible += money;
        }
        public void inputMoney(int money) //внесение денег в ящик
        {
            money_real += money;
            money_visible += money;
        }
        public void takeMoney(int money) //взятие денег из ящика
        {
            money_real-=money;
            money_visible -= money;
            if (money_visible < 0) money_visible = 0;
        }
        public int checkMoney() //показать сколько денег в ящике в теории 
        {
            return money_visible;
        }
        public int checkRealMoney() //показать сколько денег в ящике на самом деле
        {
            return money_real;
        }

    }
}
