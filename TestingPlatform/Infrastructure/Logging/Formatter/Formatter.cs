using System.Collections.Generic;
using static TestingPlatform.Infrastructure.Logging.Handlers.Base.Handler;

namespace TestingPlatform.Infrastructure.Logging.Formatter
{
    internal static class Formatter
    {

        public static string FormatString(Dictionary<string, StringDelegate> dictPairs, string stringFormat)
        {
            string resultString = stringFormat;
            foreach (KeyValuePair<string, StringDelegate> pair in dictPairs) {
                resultString = resultString.Replace(pair.Key, pair.Value());
            }
            return resultString;
        }

    }
}
