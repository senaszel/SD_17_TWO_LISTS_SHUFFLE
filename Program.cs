using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SD_17_TwoListsShuffle
{
    class Program
    {
        static void Main(string[] args)
        {
            bool vertical = false;

            if (args.Length == 0 || args[0] == "/?" || args[0] == "?" || args[0].ToUpper() == "-H" || args[0].ToUpper() == "H" || args[0].ToUpper() == "--H" || args[0] == "--help" || args[0] == "-help" || args[0] == "help")
            {
                Console.WriteLine("Witaj w programie do mieszania dwóch eeee list?");
                Console.WriteLine("Aby poprawnie uruchomić program do mieszania dwóch źródeł danych podaj ścieżki do dwóch plików \".txt\" albo \".csv\" w których wartości do zmieszania rozdzielone są przecinkiem , !");
                Console.WriteLine("Możesz także wywołać ten ekran pomocy za pomocą typowych koment pomocy -h/?/help");
                Exit(1);
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToUpper() == "V" || args[i].ToUpper() == "-V" || args[i].ToUpper() == "--V" || args[i] == "-vertical" || args[i] == "--vertical" || args[i] == "vertical") { vertical = true; continue; }

                    if (File.Exists(args[i]))
                    {
                        if (i == 0) Console.WriteLine();
                        Console.WriteLine($"{args[i]} istnieje, można kontynuować.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Jeden z podanych plików\n{args[i]}\nsprawia problemy. Sprawdz pliki i spróbuj ponownie.");
                        if (!Path.GetExtension(args[i]).EndsWith(".txt") || !Path.GetExtension(args[i]).EndsWith(".csv"))
                        { Console.WriteLine($"{args[i]} nie ma odpowiedniego rozszerzenia (txt/csv)"); }

                        Console.ResetColor();
                        Exit(1);
                    }
                }
            }

            Console.Write("\n");

            string[] res = CreateArray(args[0]);
            res = ValidateArray(res, args[0]);

            string[] res2 = CreateArray(args[1]);
            res2 = ValidateArray(res2, args[1]);


            string[] res3 = new string[res.Length + res2.Length];
            Array.Copy(res, 0, res3, 0, res.Length);
            Array.Copy(res2, 0, res3, res.Length, res2.Length);


            res3 = Shuffle(res3);

            for (int i = 0; i < res3.Length; i++)
            {
                if (vertical)
                {
                    if (i == 0) Console.WriteLine(new string('=', 35));
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(string.Concat(i.ToString(), ")").PadRight(5));
                    Console.Write(res3[i].PadLeft(26));
                    Console.ResetColor();
                    Console.WriteLine(" |");
                    Console.WriteLine(new string('=', 35));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(string.Concat(res3[i], " "));
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\n");


            Exit(0);
        }

        private static void Exit(int exitCode, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("Program zakończł się.");
            Console.ResetColor();
            Environment.Exit(exitCode);
        }

        private static string[] CreateArray(string argument)
        {
            string[] result = new string[] { };

            if (File.Exists(argument))
            {
                using StreamReader sr = new StreamReader(argument);
                result = sr.ReadToEnd().Split(',');
            }


            return result;
        }

        private static string[] Shuffle(string[] arr)
        {
            Random random = new Random();

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                int rand = random.Next(i);
                string _temp = arr[rand];
                arr[rand] = arr[i];
                arr[i] = _temp;
            }


            return arr;
        }


        private static string[] ValidateArray(string[] arr, string nameOfOriginOfData)
        {
            string[] result = new string[arr.Length];
            string pattern = @"[^a-zA-Z0-9]";
            string pattern2 = @"^[a-zA-Z0-9]+$";
            Regex regex = new Regex(pattern);
            Regex regex2 = new Regex(pattern2);

            int counter = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = regex.Replace(arr[i], "");

                if (regex2.IsMatch(arr[i]))
                {
                    result[counter] = arr[i];
                    counter++;
                }
            }
            Array.Resize(ref result, counter);

            if (result.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Walidacja stwierdza, iż wyekstrahowany z pliku {nameOfOriginOfData} ciąg znaków jest pusty.");
                Exit(1, ConsoleColor.Red);
            }


            return result;
        }



    }
}
