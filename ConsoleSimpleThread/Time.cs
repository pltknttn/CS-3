using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSimpleThread
{
    public class Time
    {
        string _text;
        int _count;

        public Time (string text, int count)
        {
            _text = text;
            _count = count;
        }

        public void Print()
        {
            var currentDateTime = DateTime.Now;
            var printCount = _count;
            while (printCount > 0)
            {
                Console.Title = $"{currentDateTime:dd.MM.yyyy HH:mm:ss.fff} {_text}";
                Thread.Sleep(100);
                currentDateTime = DateTime.Now;
                printCount--;
            }
            Console.WriteLine($"\n\rStop time: {_count}=>{printCount}");
        }
    }
}
