using System;
using System.IO;
using System.Reflection;

namespace Test2_Gulliver
{
    internal class Program
    {
        public static string InputFile = @"Samples\input.txt";
        public static string OutputFile = @"Samples\output.txt";

        static void Main(string[] args)
        {
            Execute();
        }

        public static void Execute()
        {
            long result = 0;
            if (FileExistanceChecker(GetFullPath(InputFile)))
            {
                result = GetResult(GetValue(GetFullPath(InputFile)));
                WriteToOutput(result.ToString(), GetFullPath(OutputFile));
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }

        public static bool FileExistanceChecker(string path)
        {
            return File.Exists(path);
        }

        public static string GetFullPath(string path)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(dir, path);
            return fullPath;
        }

        public static (int times, int layer) GetValue(string path)
        {
            string value = "";
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    value = sr.ReadToEnd();
                }
            }

            string[] result = value.Split(' ');
            if (result.Length == 2)
            {
                if (int.TryParse(result[0], out int times))
                {
                    if (int.TryParse(result[1], out int layer))
                    {
                        return (times, layer);
                    }
                }

            }

            return (0, 0);
        }

        public static long GetResult((int times, int layer) fields)
        {
            return (long)Math.Pow(fields.times, 2) * fields.layer;
        }

        public static void WriteToOutput(string output, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(output);
                }
            }
        }

    }
}
