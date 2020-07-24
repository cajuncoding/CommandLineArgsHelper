using CommandLineArgsHelpers;
using Newtonsoft.Json;
using System;

namespace CommandLineArgsHelperConsoleApp
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
                //First load and output the command args Dynamically (Loosely Coupled)...
                Console.WriteLine("Dynamically processing the Command Line Args...");
                Console.WriteLine($"Command Line Arguments [{commandLineArgHelper.Parameters.Count} found]:");
                var x = 0;
                foreach (var kv in commandLineArgHelper.Parameters)
                {
                    Console.WriteLine($"   {++x}) [{kv.Key}] == [{kv.Value}]");
                }

                Console.WriteLine();
                Console.WriteLine("Press Enter to see Strongly Typed Command Class sample (rendered as Serialized Json)...");
                Console.ReadLine();

                //Now Load and render out the Strongly Typed Command Arg (Static/Tightly Coupled)...
                Console.WriteLine();
                Console.WriteLine($"Statically processing the Command Line Args (Serialized as Json)...");

                var myCommandArgs = new MyCommandArgs(commandLineArgHelper);
                var myCommandArgsJson = JsonConvert.SerializeObject(myCommandArgs, Formatting.Indented);

                Console.WriteLine($"{nameof(MyCommandArgs)}:");
                Console.WriteLine(myCommandArgsJson);

                Console.WriteLine();
                Console.WriteLine("Finished.");
            }
            else
            {
                Console.Write("No command line arguments were specified, please try again.");
            }

            Console.ReadLine();

        }
    }
}
