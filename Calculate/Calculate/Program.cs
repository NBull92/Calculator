using CoreLibrary;
using System;
using Math = CoreLibrary.Math;

namespace Calculate
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Question One ---");
            Console.WriteLine(Calculate("2 + 5",7));
            Console.WriteLine(Calculate("8 - 3",5));
            Console.WriteLine(Calculate("5 * 4",20));
            Console.WriteLine(Calculate("8 / 2",4));
            Console.WriteLine(Calculate("4 ^ 2",16));
            
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("--- Question Two ---");
            Console.WriteLine(Calculate("1 + 2 * 3",7));
            Console.WriteLine(Calculate("(1 + 2) * 3",9));
            Console.WriteLine(Calculate("6 + 3 - 2 + 12", 19));
            Console.WriteLine(Calculate("2 * 15 + 23",53));
            Console.WriteLine(Calculate("10 - 3 ^ 2",1));

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("--- Question Three ---");
            Console.WriteLine(Calculate("3.5 * 3",10.5));
            Console.WriteLine(Calculate("-53 + -24",-77));
            Console.WriteLine(Calculate("10 / 3 ",3.333));
            Console.WriteLine(Calculate("(-20 * 1.8) / 2",-18));
            Console.WriteLine(Calculate("-12.315 - 42",-54.315));

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("--- Extras ---");
            Console.WriteLine(Calculate("80 - 2", 78));
            Console.WriteLine(Calculate("8  + 10", 18));
            Console.WriteLine(Calculate("(", 0));
            Console.WriteLine(Calculate("(()", 0));
            Console.WriteLine(Calculate("(((1 + 2) * 3)", 0));
            Console.WriteLine(Calculate("((1 + 2)) * 3)", 0));

            Console.ReadKey();
        }

        private static string Calculate(string expression, double expected)
        {
            try
            {
                var result = Math.Calculate(expression);
                return $"Answer: {result} \tExpected: {expected}\n";
            }
            catch (InvalidExpressionException ex)
            {
                return ex.Message;
            }
        }
    }
}
