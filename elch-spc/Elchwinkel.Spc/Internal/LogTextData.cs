using System.Collections.Generic;
using System.IO;

namespace Elchwinkel.Spc.Internal
{
    internal class LogTextData
    {
        public LogTextData(string str)
        {
            Raw = str;
            KeyValues = new Dictionary<string, string>();
            using (var sr = new StringReader(str))
            {
                var l = sr.ReadLine();
                while (l != null)
                {
                    var parts = l.Split('=');
                    if (parts.Length != 2) break;
                    KeyValues.Add(parts[0].Trim(), parts[1].Trim());
                    l = sr.ReadLine();
                }
            }
        }

        public string Raw { get; }
        public IDictionary<string, string> KeyValues { get; }
        public static LogTextData Empty => new LogTextData(string.Empty);
    }
}