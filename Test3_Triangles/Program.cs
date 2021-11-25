using System;
using System.IO;
using System.Reflection;

namespace Test3_Triangles
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

        public static int GetValue(string path)
        {
            string value = "";
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    value = sr.ReadToEnd();
                }
            }

            if (int.TryParse(value, out int f))
            {
                return f;
            }

            return 0;
        }

        public static long GetResult(int floors)
        {
            if (floors == 1)
                return 1;
            else if (floors < 1)
                return 0;

            long tinyTris = 0;
            long biggerTris = 0;

            bool calculatedTinyTris = false;

            int unitTri = 1;

            // Calculating 
            while (unitTri != floors)
            {
                for (int index = 1; index <= floors; index++)
                {
                    // Calculating tiny trianges 
                    if (!calculatedTinyTris)
                    {
                        var value = 2 * index - 1;
                        tinyTris += value;
                        continue;
                    }

                    // Calculating bigger triangles
                    if (index + unitTri <= floors + 1)
                    {
                        biggerTris += index;
                    }

                    // Calculating bigger triangles (reversed ones)
                    if (index >= unitTri && index + unitTri <= floors + 1)
                    {
                        biggerTris += index / unitTri;
                    }
                }
                unitTri++;
                calculatedTinyTris = true;
            }
            Console.WriteLine(tinyTris);
            return tinyTris + biggerTris;
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
