using CommandLineArgsHelpers;
using System;

namespace CommandLineArgsHelperConsoleApp
{
    /// <summary>
    /// Sample of a mapping into a Strongly Typed Command Arg class for pre-defined args that you would like to validate this way.
    /// </summary>
    public class MyCommandArgs
    {
        private CommandLineArgsHelper args = null;

        public MyCommandArgs(CommandLineArgsHelper args)
        {
            this.args = args ?? throw new ArgumentNullException(nameof(args), "Command Line Parameters collection must be specified.");
        }

        public string Size => args["size"] ?? throw new ArgumentNullException("size", "The Size argument is Required!"); //Manual Required field via Null-Coalesce
        public string Height => args["height"] ?? String.Empty; //Optional; default is empty string.
        public string Day => args["day"]; //Optional; default is null
        public string Tgif => args["tgif"].AssertArgumentNotNull("tgif"); //Required Parameter using Custom Extension Method.
        public string AmINice => args["AmINice"];
        public bool Debug => String.Equals(args["debug"], "true", StringComparison.OrdinalIgnoreCase); //Boolean value that must be 'true' to be True; case-insensitive.
        public string Param1 => args["param1"];
        public string Param2Alone => args["param2Alone"];
        public string Param3 => args["param3"];
        public string Param4 => args["param4"];
        public string Param6_with_spaces => args["param 6 with spaces"];
    }
}
