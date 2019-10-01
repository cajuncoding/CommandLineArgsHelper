using System;

namespace CommandLineArgsHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLneArgHelper = new CommandLineArgsHelper(args);

            if (commandLneArgHelper.Parameters.Count > 0)
            {
                Console.WriteLine($"Command Line Arguments:");
                var x = 0;
                foreach (var kv in commandLneArgHelper.Parameters)
                {
                    Console.WriteLine($"   {++x}) [{kv.Key}] == [{kv.Value}]");
                }
            }
            else
            {
                Console.Write("No command line arguments were specified, please try again.");
            }

            Console.ReadLine();
        }
    }
}
