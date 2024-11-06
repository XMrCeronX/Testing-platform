using System;
using System.Collections.Generic;

namespace TestingPlatform.Infrastructure.Logging.Formatter
{
    internal static class Formatter
    {
        public static string ReplaceStringsToValues(Dictionary<string, Func<string>> replaceableDict, string stringFormat)
        {
            string resultString = stringFormat;
            foreach (KeyValuePair<string, Func<string>> pair in replaceableDict) {
                resultString = resultString.Replace(pair.Key, pair.Value());
            }
            return resultString;
        }
    }
}
