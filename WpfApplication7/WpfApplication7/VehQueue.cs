using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication7
{
    public class VehQueue
    {
        Queue<Vehicle> vehicleQ;
        public VehQueue()
        {
            vehicleQ = new Queue<Vehicle>();
        }
        public void Add() //добавление элемента в очередь
        {
            vehicleQ.Enqueue(new Vehicle());
        }
        public void Delete() //удаление элемента из очереди
        {
            vehicleQ.Dequeue();
        }
        public int Count() //количество элементов в очереди
        {
            return vehicleQ.Count;
        }
        public int GetClassZero() //класс первого элемента
        {
            return vehicleQ.First().getClass();            
        }
        public int GetNumZero()
        {
            return vehicleQ.First().num;
        }
        public void SetClassZero(int a) //установка класса первого элеммента
        {
            vehicleQ.First().setClass(a);
        }
    }
}
