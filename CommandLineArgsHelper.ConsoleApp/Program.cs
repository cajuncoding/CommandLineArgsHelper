using System;

namespace CommandLineArgsHelper
{
    class Program
    {
        //BBernard
        //Command Line Args can be defined in the Debug Tab of the Console App Properties...
        static void Main(string[] args)
        {
            var commandLineArgHelper = new CommandLineArgsHelper(args);

            if (commandLineArgHelper.Parameters.Count > 0)
            {
                Console.WriteLine($"Command Line Arguments:");
                var x = 0;
                foreach (var kv in commandLineArgHelper.Parameters)
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
