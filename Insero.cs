using System;
using System.Collections.Generic;
namespace PokemonPocket
{
    public class Insero
    {
        public static int PromptInt(string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int inp = Int32.Parse(Console.ReadLine());
                    return inp;
                }
                catch
                {
                    Console.WriteLine("That is not a number!");
                }
            }
        }
        public static int PromptInt(string prompt, string err)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int inp = Int32.Parse(Console.ReadLine());
                    return inp;
                }
                catch
                {
                    Console.WriteLine(err);
                }
            }
        }
        public static int PromptInt(string prompt, int min, int max)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int inp = Int32.Parse(Console.ReadLine());
                    if ((inp >= min) && (inp <= max))
                    {
                        return inp;
                    }
                    throw new Exception();
                }
                catch
                {
                    Console.WriteLine($"Please enter a number between {min} and {max}");
                }
            }
        }
        public static int PromptInt(string prompt, string err, int min, int max)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int inp = Int32.Parse(Console.ReadLine());
                    if ((inp >= min) && (inp <= max))
                    {
                        return inp;
                    }
                    throw new Exception();
                }
                catch
                {
                    Console.WriteLine(err);
                }
            }
        }
        public static int PromptInt(string prompt, List<string> options)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {options[i]}");
            }
            while (true)
            {
                try
                {
                    Console.Write(">>> ");
                    int inp = Int32.Parse(Console.ReadLine());
                    if ((inp >= 1) && (inp <= options.Count))
                    {
                        return inp - 1;
                    }
                    throw new Exception();
                }
                catch
                {
                    Console.WriteLine($"Please enter a number between 1 and {options.Count}");
                }
            }
        }
        public static string PromptString(string prompt)
        {
            Console.WriteLine(prompt);
            Console.Write(">>> ");
            return Console.ReadLine();
        }
        public static string PromptString(string prompt, List<string> blacklist)
        {
            Console.WriteLine(prompt);
            Console.Write(">>> ");
            string inp = Console.ReadLine();
            while (blacklist.Contains(inp))
            {
                Console.WriteLine($"'{inp}' is not allowed!");
                Console.Write(">>> ");
                inp = Console.ReadLine();
            }
            return inp;
        }
        public static string PromptString(string prompt, string err, List<string> blacklist)
        {
            Console.WriteLine(prompt);
            Console.Write(">>> ");
            string inp = Console.ReadLine();
            while (blacklist.Contains(inp))
            {
                Console.WriteLine(err);
                Console.Write(">>> ");
                inp = Console.ReadLine();
            }
            return inp;
        }
        public static string PromptString(List<string> options, string prompt)
        {
            Console.WriteLine(prompt);
            options.ForEach(el => Console.WriteLine(el));
            Console.Write(">>> ");
            string inp = Console.ReadLine();
            while (!(options.Contains(inp)))
            {
                Console.WriteLine($"'{inp}' is not one of the options!");
                Console.Write(">>> ");
                inp = Console.ReadLine();
            }
            return inp;
        }
    }
}