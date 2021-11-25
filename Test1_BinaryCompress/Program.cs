using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Test1_BinaryCompress
{
    internal class Program
    {
        public static string InputPath = @"Samples\input.txt";
        public static string OutputPath = @"Samples\output.txt";

        static void Main(string[] args)
        {
            Execute();
        }

        public static void Execute()
        {
            var inputPath = GetFullPath(InputPath);
            var outputPath = GetFullPath(OutputPath);

            string result = default;
            if(FileExistanceChecker(inputPath))
            {
                result = GetString(inputPath);
            }

            string pattern = GetPattern(result);
            WriteToOutput(pattern, outputPath);
        }

        public static bool FileExistanceChecker(string path)
        {
            if (File.Exists(path))
                return true;

            return false;
        }

        public static string GetFullPath(string path)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dir, path);
        }

        public static string GetString(string filePath)
        {
            string result = default;
            using (FileStream fs = File.Open(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }

        public static string GetPattern(string input)
        {
            string result = "";
            byte zerosCount = 0;
            foreach(char symbol in input)
            {
                if(symbol == '0')
                {
                    zerosCount++;
                    continue;
                }
                if (symbol == '1')
                {
                    result += GetSymbolByPattern(zerosCount);
                    zerosCount = 0;
                }
            }
            return result;
        }

        public static string GetSymbolByPattern(int numberOfZeros)
        {
            string result = string.Empty;
            var zeros = numberOfZeros;
            if(numberOfZeros > 25)
            {
                zeros = (numberOfZeros - 25);
                result += new String('0', zeros);
                zeros = 25;
            }

            char symbol = (char)(zeros + 'a');
            result += symbol;
            return result;
        }

        public static void WriteToOutput(string output, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(output);
                }
            }
        }
    }
}
