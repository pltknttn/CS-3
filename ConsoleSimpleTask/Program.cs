using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSimpleTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"TestTask");
            TestTask();
            
            Console.WriteLine($"TestTaskFactory");
            TestTaskFactory();

            Console.WriteLine($"TestMultiTask");
            TestMultiTask();

            Console.WriteLine($"async\\await Work");
            Work();

            Console.WriteLine($"Stop");
            Console.ReadLine();
        }

        static void TestMultiTask()
        { 
            var t1 = Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                Console.WriteLine($"Метод запущен из потока {t.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"Метод завершен в потоке {t.ManagedThreadId}");
            });

            var t2 = Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                Console.WriteLine($"Метод запущен из потока {t.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Метод завершен в потоке {t.ManagedThreadId}");
            });

            var t3 = Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                Console.WriteLine($"Метод запущен из потока {t.ManagedThreadId}");
                Thread.Sleep(3000);
                Console.WriteLine($"Метод завершен в потоке {t.ManagedThreadId}");
            });
             
            //хотя бы
            var i = Task.WaitAny(t1, t2, t3);

            Console.WriteLine($"Готов {i}");            
            Console.ReadLine();
        }

        static void TestTaskFactory()
        {
            var task = Task.Factory.StartNew(obj => ParallelMethod((string)obj), "Hello world");

            task.ContinueWith(o =>
            {
                Console.WriteLine($"Метод ContinueWith");
            }, TaskContinuationOptions.None);

            Console.ReadLine();
        }

        static void TestTask()
        {
            var mainThread = Thread.CurrentThread;
            Console.WriteLine($"Главный поток = {mainThread.ManagedThreadId}");

            //var task = new Task(ParallelMethod);
            //task.Start();

            var task = Task.Run(() =>
            {
                ParallelMethod();
                return 100;
            });


            Console.WriteLine("Перед wait");
            task.Wait();
            Console.WriteLine("После wait");

            task.ContinueWith(o =>
            {
                Console.WriteLine($"После вызова метода, Result = {o.Result}");
            });

            Console.ReadLine();
        }

        static void ParallelMethod(string text)
        {
            var t = Thread.CurrentThread;
            Console.WriteLine($"Метод запущен из потока {t.ManagedThreadId}, text={text}");

            Thread.Sleep(2000);
            Console.WriteLine($"Метод завершен в потоке {t.ManagedThreadId}, text={text}");
        }

        static void ParallelMethod()
        {
            var t = Thread.CurrentThread;
            Console.WriteLine($"Метод запущен из потока {t.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"Метод завершен в потоке {t.ManagedThreadId}");
        }

        static async void Work()
        {
            await SomeThingAsync();
        }

        static async Task SomeThingAsync()
        {
            var currThread = Thread.CurrentThread;
            Console.WriteLine($"Перед async потоком {currThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                var mainThread = Thread.CurrentThread;
                Console.WriteLine($"Из async потока {mainThread.ManagedThreadId}");
            });

            Console.WriteLine($"После async потока {currThread.ManagedThreadId}");
        }
    }
}

