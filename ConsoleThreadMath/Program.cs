using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleThreadMath
{
    class Program
    {
        static object lockCalc = new object();
        static object lockConsole = new object();
        
        static void Main(string[] args)
        {
            int x = 0;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 2);
                Console.Write("Введите целое число X для расчета Факториал(Х) и СуммаЧисел(Х): ");
            }
            while (!int.TryParse(Console.ReadLine(), out x));
            Console.WriteLine();

            var currentCursorPosition = 3;
            var threadFact = new Thread((n) => {                
                var result = "Только для натуральных чисел!!!";
                if (long.TryParse(n.ToString(), out long p) && p >=0)
                {
                    result = Factorial(p).ToString();
                }                
                lock (lockConsole)
                { 
                    Console.SetCursorPosition(0, currentCursorPosition++);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\rФакториал({n}) = {result}");
                    currentCursorPosition++;
                }
            }) { IsBackground = true, Priority = ThreadPriority.Highest, Name = "Факториал"};

            var threadSum = new Thread((n) =>
            {
                var sum = Sum((int)n);
                lock (lockConsole)
                { 
                    Console.SetCursorPosition(0, currentCursorPosition++);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"\n\rСуммаЧисел({n}) = {sum}");
                    currentCursorPosition++;
                }
            }) { IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = "СуммаЧисел" };

            bool startThread = false;
            bool isCalc = true;
            do
            {                
                if (!startThread)
                {
                    Console.SetCursorPosition(0, currentCursorPosition++);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Приступили к вычислениям {DateTime.Now: HH.mm.ss.fff}...");
                    threadFact.Start(x);
                    threadSum.Start(x);
                    startThread = true;
                    continue;
                }
                Thread.Sleep(100);
                isCalc = threadFact.IsAlive || threadSum.IsAlive;
                if (isCalc)
                {
                    lock (lockConsole)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(0, currentCursorPosition);
                        Console.Write($"Ожидаем завершения вычислений {DateTime.Now: HH.mm.ss.fff}...");
                    }
                }
            }
            while (isCalc);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(0, currentCursorPosition+1);
            Console.WriteLine($"Вычисления завершены {DateTime.Now: HH.mm.ss.fff}. \n\rНажмите Enter для выхода");
            Console.ReadLine();
        }

        static long Factorial(long x)
        {
            lock (lockCalc)
            {
                if (x <= 0) return 1;
                Thread.Sleep(200);
                return x * Factorial(x - 1);
            }
        }

        static int Sum(int x)
        {
            lock(lockCalc)
            {
                if (x == 0) return 0;
                Thread.Sleep(200);
                return x > 0 ? x + Sum(x - 1) : x + Sum(x + 1);
            }
        }
    }
}
