using System;
using System.IO;
using System.Reflection;

namespace Task5_Titanic
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
            if (FileExistanceChecker(GetFullPath(InputFile)))
            {
                string message = GetResult(GetValue(GetFullPath(InputFile)));
                WriteToOutput(message, GetFullPath(OutputFile));
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

        public static Location[] GetValue(string path)
        {
            Location GetLocationFromString(params string[] valuesString)
            {
                if (valuesString.Length < 2)
                    return null;

                var values = new (int d, int m, int s, Poles pole)[valuesString.Length]; // { d = 0, m = 0, s = 0, pole = Poles.N };
                for (int index = 0; index < valuesString.Length; index ++)
                {
                    valuesString[index] = valuesString[index].Trim();
                    valuesString[index] = valuesString[index].Replace('^', ' ');
                    valuesString[index] = valuesString[index].Replace('\'', ' ');
                    valuesString[index] = valuesString[index].Replace('\"', ' ');
                    string[] valArr = valuesString[index].Split(' ');
                    int d = int.Parse(valArr[0]);
                    int m = int.Parse(valArr[1]);
                    int s = int.Parse(valArr[2]);
                    Poles pole = (Poles)Enum.Parse(typeof(Poles), valArr[4]);
                    values[index] = (d, m, s, pole);
                }

                if(values[0].pole == Poles.N || values[0].pole == Poles.S)
                {
                    return new Location(values[0].d, values[0].m, values[0].s, values[0].pole, values[1].d, values[1].m, values[1].s, values[1].pole);
                } else
                {
                    return new Location(values[1].d, values[1].m, values[1].s, values[1].pole, values[0].d, values[0].m, values[0].s, values[0].pole);
                }
            }

            string value = "";
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    value = sr.ReadToEnd();
                }
            }

            string[] coordinates = new string[4];
            Location[] locations = new Location[2];
            int coorIndex = 0;
            string fixedValue = value.Replace("\r\n", " ");
            int startIndex = 0, endIndex = 0;

            for (int index = 0; index < fixedValue.Length; index++)
            {
                // Find the start index after matching this symbol
                if (fixedValue[index] == '^' && startIndex == 0)
                {
                    for (int indexA = index - 1; indexA >= 0; indexA--)
                        if (!char.IsDigit(fixedValue[indexA]))
                        {
                            startIndex = indexA;
                            break;
                        }
                }

                if (fixedValue[index] == '"')
                {
                    index = (index + 1 < fixedValue.Length) ? index + 1 : index;
                    for (int indexA = index; indexA <= fixedValue.Length; indexA++)
                        if (!char.IsDigit(fixedValue[indexA]) && fixedValue[indexA] != ' ')
                        {
                            char symbol = fixedValue[indexA];
                            if (fixedValue[indexA] == 'N' || fixedValue[indexA] == 'S' || fixedValue[indexA] == 'W' || fixedValue[indexA] == 'E')
                            {
                                endIndex = indexA + 1;
                                coordinates[coorIndex++] = fixedValue.Substring(startIndex, endIndex - startIndex);
                                startIndex = 0;
                                endIndex = 0;
                                break;
                            }
                            else
                            {
                                throw new ArgumentException("String format is invalid");
                            }
                        }
                }
            }

            locations[0] = GetLocationFromString(coordinates[0], coordinates[1]);
            locations[1] = GetLocationFromString(coordinates[2], coordinates[3]);
            return locations;
        }

        public static string GetResult(Location[] locations)
        {
            if (locations.Length < 2)
                return "Error calculating distance";
            var distance = DistanceCalculator.Calculate(locations[0], locations[1]);
            var message = $"The distance to the iceberg: {distance} miles";
            if (distance < 100)
                message += "\nDANGER";
            return message;
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
