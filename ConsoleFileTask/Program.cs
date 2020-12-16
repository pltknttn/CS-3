using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleFileTask
{
    class Program
    {
        public static void ClearCurrentConsoleLine(int cursorPosition)
        {
            int currentLineCursor = cursorPosition;
            Console.SetCursorPosition(0, cursorPosition);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Действия над файлами");
            string fileDir = string.Empty;
            do
            {
                ClearCurrentConsoleLine(1);
                Console.Write("Укажите директорию поиска: ");
                fileDir = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(fileDir) || !Directory.Exists(fileDir));

            var taskReadDir = Task.Run(() =>
            {
                ProcessDirectory(fileDir);
                return fileSearchers.Count;
            })
              .ContinueWith(o =>
            {
                Console.WriteLine($"ReadDir Result: \n\r{o.Result} files");
            });

            var taskParseFile = Task.Run(() =>
            {
                var stringBuilder = new StringBuilder();
                while (!taskReadDir.IsCompleted)
                {
                    if (ParseFiles(out var res)) stringBuilder.AppendLine(res.Trim("\n\r".ToCharArray()));
                }
                return stringBuilder.ToString();
            })
                .ContinueWith(o =>
            {
                Console.WriteLine($"ParseFile Result: \n\r{o.Result}");
            });
                       
            Task.WaitAll(taskReadDir, taskParseFile);

            Console.WriteLine($"Stop");
            Console.WriteLine();
            Console.ReadLine();
        }

        static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries) ReadFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries) ProcessDirectory(subdirectory);
        }

        static List<FileSearcher> fileSearchers = new List<FileSearcher>();
        static void ReadFile(string path)
        {            
            fileSearchers.Add(new FileSearcher { File = path, Content = File.ReadAllText(path), IsProcessed = true });
        }
         
        static Regex _regex = new Regex(@"^([12]{1})\s([0-9]+([.,][0-9]+)?)\s([0-9]+([.,][0-9]+)?)$");
        static bool ParseFiles(out string result)
        {
            result = null;            
            var workFiles = fileSearchers.FindAll(x => x?.IsProcessed??false);
            if (workFiles.Any())
            {
                var stringBuilder = new StringBuilder();
                var count = workFiles
                    .AsParallel().WithDegreeOfParallelism(1)
                    .Where(file =>
                    {
                        file.IsProcessed = false;
                        var collMatches = _regex.Matches(file.Content);
                        if (collMatches == null || collMatches.Count == 0) return false;

                        var action = int.Parse(collMatches[0].Groups[1].Value);
                        var num1 = decimal.Parse(collMatches[0].Groups[2].Value?.Replace('.', ','));
                        var num2 = decimal.Parse(collMatches[0].Groups[4].Value?.Replace('.', ','));
                        var sum = action == 1 ? num1 * num2 : num2 != 0 ? num1 / num2 : 0.0M;

                        stringBuilder.AppendLine($"file='{file.File}',result='{sum}'");
                        
                        return true;
                    })?.Count();

                result = stringBuilder.ToString();

                return count > 0;
            }
            return false;
        }

        internal class FileSearcher
        {
            public string File { get; set; }
            public string Content { get; set; }
            public bool IsProcessed { get; set; } 
        }
    }
}
