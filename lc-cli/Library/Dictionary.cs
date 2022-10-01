using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.Library
{
    public class Dictionary
    {
        public Dictionary<string, string> Library { get; set; } = new();

        public string this[string function]
        {
            get
            {
                return Library.ContainsKey(function) ? Library[function] : String.Empty;
            }
        }

        public void Add(string name, string function)
        {
            Library.Add(name, function);
        }
    }
}
