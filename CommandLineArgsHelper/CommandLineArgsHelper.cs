using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CommandLineArgsHelpers
{
    #region Helper Class for parsing and processing Command Line Arguments

    /// <summary>
    /// BBernard
    /// Command Line Arguments Parser Class
    /// NOTE: Code updated and simplified by BBernard
    ///     - Updated with new C# syntax
    ///     - Enhanced class wrapper (e.g.exposed read only dictionary for enumeration)
    ///     - More robust parsing for wider array of parameter syntaxes
    ///     - Support for both key/value parameters as well as simple boolean flags(param is defined or not)
    ///     - Improved thread safety
    ///     - Eliminated code duplication
    /// NOTE: Adapted from original source located at: 
    ///       https://www.codeproject.com/Articles/3111/C-NET-Command-Line-Arguments-Parser
    /// </summary>
    public class CommandLineArgsHelper
    {
        // Variables
        private static readonly Regex _splitterRegex = new Regex(@"^-{1,2}|^\/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex _cleanerRegex = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        //Ensure that the underlying data is read only for thread safety
        public IReadOnlyDictionary<string, string> Parameters { get; }

        #region Constructors
        public CommandLineArgsHelper()
            : this(Environment.GetCommandLineArgs())
        {}

        public CommandLineArgsHelper(string[] argsArray)
        {
            var paramsDictionary = ParseArgumentsHelper(argsArray);
            Parameters = new ReadOnlyDictionary<string, string>(paramsDictionary);
        }
        #endregion

        private Dictionary<string, string> ParseArgumentsHelper(string[] argsArray)
        {
            //Always return a valid/initialized Dictionary
            var paramsDictionary = new Dictionary<string, string>();

            //Validate and short-circuit
            if (argsArray == null || argsArray.Length == 0) return paramsDictionary;

            string lastParam = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            //  -size=100 /height:'400' /day:"happy birthday" -tgif="Thank God It's Friday" 
            //  -AmINice "Nice stuff !" --debug -param1 value1 --param2Alone /param3:"Test-:-work"
            //  /param4=--value4-- -param5 "--=nice=--"
            foreach (var text in argsArray)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                var partsArray = _splitterRegex.Split(text, 3);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (partsArray.Length)
                {
                    case 1: // Found a value for the last parameter or found a space separator
                        TryInitParamInDictionary(paramsDictionary, lastParam, partsArray[0]);
                        lastParam = null;
                        // Else the parameter waiting for a value so we skip and keep looking...
                        break;

                    case 2: // Found just a parameter
                        // The last parameter is still waiting with no value, we initialize it to true.
                        TryInitParamInDictionary(paramsDictionary, lastParam);
                        // Log the single Param found, and keep looking to see if it has a value
                        lastParam = partsArray[1];
                        break;

                    case 3: // Parameter with enclosed value

                        // Remove possible enclosing characters (",')
                        if (!TryInitParamInDictionary(paramsDictionary, partsArray[1], partsArray[2]))
                        {
                            //If we are unable to parse a Name & Value, then we use the current parameter
                            //  as the value of the last parameter.
                            TryInitParamInDictionary(paramsDictionary, lastParam, text);
                        }
                        else
                        {
                            // The last parameter is still waiting with no value, we initialize it to true.
                            TryInitParamInDictionary(paramsDictionary, lastParam);
                        }
                        lastParam = null;
                        break;
                }
            }

            // In case a parameter is still waiting
            TryInitParamInDictionary(paramsDictionary, lastParam);

            //Return the populated dictionary
            return paramsDictionary;
        }

        private bool TryInitParamInDictionary(Dictionary<string, string> paramDictionary, string parameter, string value = "true")
        {
            //Only add/initialize the value if it is not null and does not already exist.
            if (!string.IsNullOrEmpty(parameter) && !paramDictionary.ContainsKey(parameter))
            {
                var cleanedValue = _cleanerRegex.Replace(value, "$1");
                paramDictionary.Add(parameter, cleanedValue);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Implement indexer to retrieve values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name] => Parameters.GetValueOrDefault(name);
    }
    #endregion

    #region Custom Extension Helpers

    /// <summary>
    /// BBernard
    /// Custom Extension methods to make working with Dictionary objects easier
    /// </summary>
    public static class CustomExtensionHelpers
    {

        public static T AssertArgumentNotNull<T>(this T argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return argument;
        }

        /// <summary>
        /// BBernard
        /// Supports exception-safe retrieval of values from a Dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> dictionary, K key, V defaultValue = default(V))
        {
            dictionary.AssertArgumentNotNull(nameof(dictionary));
            key.AssertArgumentNotNull(nameof(key));

            var result = dictionary.TryGetValue(key, out var value) ? value : defaultValue;
            return result;
        }
    }
    #endregion

}
