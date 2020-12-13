using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleConverterCsvToTxt
{
    class Program
    {
        static object lockLists = new object();
        static object lockConsole = new object();

        public static void ClearCurrentConsoleLine(int cursorPosition)
        {
            int currentLineCursor = cursorPosition;
            Console.SetCursorPosition(0, cursorPosition);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Конвертация csv=>txt");
            string fileFrom = string.Empty;
            do
            {
                ClearCurrentConsoleLine(1);
                Console.Write("Укажите csv файл: ");
                fileFrom = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(fileFrom) || !fileFrom.EndsWith(".csv", StringComparison.CurrentCultureIgnoreCase) || !File.Exists(fileFrom));

            string fileTo = string.Empty;
            do
            {
                ClearCurrentConsoleLine(2); 
                Console.Write("Укажите txt файл: ");
                fileTo = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(fileTo) || !fileTo.EndsWith(".txt", StringComparison.CurrentCultureIgnoreCase));

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Для выхода нажмите Enter");

            List<string> lists = new List<string>();
            var currentCursorPosition = 5;
                        
            var threadRead = new Thread((p) => {
                var fileName = p as string;
                lock (lockLists)
                {
                    var result = LoadCsv(fileName, out lists, out var error) ? $"Успешно cчитали {lists.Count} записей!" : "Не успешно";
                    lock (lockConsole)
                    {
                        Console.SetCursorPosition(0, currentCursorPosition++);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n\rСчитали {fileName} = {result}:{error}".TrimEnd(':'));
                        currentCursorPosition++;
                    }
                }
            })
            { IsBackground = true, Priority = ThreadPriority.Highest, Name = "Чтение" };
                        
            var threadWrite = new Thread((p) =>
            {                
                while (threadRead.IsAlive) 
                { 
                    //ждем пока считает данные
                }
                var fileName = p as string;
                var result = WriteCsv(fileName, lists, out var error)? $"Успешно" : "Не успешно";
                lock (lockConsole)
                {
                    Console.SetCursorPosition(0, currentCursorPosition++);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"\n\rЗапись в файл {fileName} завершена = {result}:{error}".TrimEnd(':'));
                    currentCursorPosition++;
                }
            })
            { IsBackground = true, Priority = ThreadPriority.BelowNormal, Name = "Запись" };

            var threadStop = new Thread(() =>
            {
                try
                {
                    Console.ReadLine();
                    if (threadRead.IsAlive || threadWrite.IsAlive)
                        Environment.Exit(0);
                }
                catch(ThreadInterruptedException)
                {
                    //прервали поток
                }
            })
            { IsBackground = true, Priority = ThreadPriority.Highest, Name = "Стоп" };

            bool startThread = false;
            bool isCalc = true;
            do
            {
                if (!startThread)
                {
                    Console.SetCursorPosition(0, currentCursorPosition++);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"Приступили к конвертации {DateTime.Now: HH.mm.ss.fff}...");
                    threadStop.Start();
                    threadRead.Start(fileFrom);
                    threadWrite.Start(fileTo);
                    startThread = true;
                    continue;
                }
                Thread.Sleep(100);
                isCalc = threadRead.IsAlive || threadWrite.IsAlive;
                if (isCalc)
                {
                    lock (lockConsole)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(0, currentCursorPosition);
                        Console.Write($"Ожидаем завершения конвертации {DateTime.Now: HH.mm.ss.fff}...");
                    }
                }
            }
            while (isCalc);
             
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(0, currentCursorPosition + 1); 
            Console.WriteLine($"Конверация завершена {DateTime.Now: HH.mm.ss.fff}. \n\rНажмите Enter для выхода");

            threadStop.Interrupt();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }

        private static bool LoadCsv(string filename, out List<string> list, out string error)
        {
            Thread.Sleep(1000);
            error = string.Empty;
            list = new List<string>();
            StreamReader sr = null; 
            try
            {
                sr = new StreamReader(filename);
                while (!sr.EndOfStream)
                {
                    try
                    {
                        list.Add(sr.ReadLine()); 
                    }
                    catch (Exception e)
                    {
                        error = e.Message;
                        return false;
                    }
                }
            }
            catch
            {
                list.Clear();
                return false;
            }
            finally
            {
                sr?.Close();
            } 
            return true;
        }

        private static bool WriteCsv(string fileName, List<string> lists, out string error)
        {
            Thread.Sleep(1000);
            error = string.Empty;
            StreamWriter sr = null;
            try
            {
                sr = new StreamWriter(fileName);
                foreach (string l in lists)
                { 
                    sr.WriteLine(l);
                }
                return true;
            }
            catch(Exception e)
            {
                error = e.Message;
                return false;
            }
            finally
            {
                sr?.Close();
            }
        }

    }
}
