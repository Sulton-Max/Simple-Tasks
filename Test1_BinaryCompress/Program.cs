using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Test1_BinaryCompress
{
    internal class Program
    {
        public static string FilePath = @"Samples\sample.txt";

        static void Main(string[] args)
        {
            Execute();
        }

        public static void Execute()
        {
            var correctPath = GetCorrectPath(FilePath);
            string result = default;
            if(FileExistanceChecker(correctPath))
            {
                result = GetString(correctPath);
            }

            string pattern = GetPattern(result);
            Console.WriteLine(pattern);
        }

        public static bool FileExistanceChecker(string path)
        {
            if (File.Exists(FilePath))
                return true;

            return false;
        }

        public static string GetCorrectPath(string path)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(dir, FilePath);
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
            }

            char symbol = (char)(zeros + 'a');
            result += symbol;
            return result;
        }
    }
}
