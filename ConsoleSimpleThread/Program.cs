using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSimpleThread
{
    class Program
    {
        static void Main(string[] args)
        {
            FamiliarityAtCurrentThread();
            ThreadTimer();
            
            Console.WriteLine("Exit key = N");

            while (Console.ReadKey().KeyChar != 'N') { Console.WriteLine(); }

            Console.WriteLine("\n\rExit...");
            Thread.Sleep(1000);
        }

        static void FamiliarityAtCurrentThread()
        {
            var currentThread = Thread.CurrentThread;  

            Console.WriteLine($"id={currentThread.ManagedThreadId}, name={currentThread.Name} priority={currentThread.Priority}");
            currentThread.Name = "Base thread";
            Console.WriteLine($"id={currentThread.ManagedThreadId}, name={currentThread.Name} priority={currentThread.Priority}");
        }

        static void PrintTime()
        {
            var currentDateTime = DateTime.Now;
            while (true)
            {
                Console.Title = $"{currentDateTime:dd.MM.yyyy HH:mm:ss.fff}";
                Thread.Sleep(100);
                currentDateTime = DateTime.Now;
            }
        }

        static void ThreadTimer()
        {
            var timer = new Thread(new ThreadStart(PrintTime))
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };
            timer.Start();
        }
    }
}
