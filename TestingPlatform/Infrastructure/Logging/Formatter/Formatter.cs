using System.Collections.Generic;

namespace TestingPlatform.Infrastructure.Logging.Formatter
{
    internal static class Formatter
    {
        public delegate string StringDelegate();

        public static string ReplaceStringsToValues(Dictionary<string, StringDelegate> replaceableDict, string stringFormat)
        {
            string resultString = stringFormat;
            foreach (KeyValuePair<string, StringDelegate> pair in replaceableDict) {
                resultString = resultString.Replace(pair.Key, pair.Value());
            }
            return resultString;
        }
    }
}
