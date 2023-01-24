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
            set
            {
                Library[function] = value;
            }
        }

        public void Add(string name, string function)
        {
            if (Library.ContainsKey(name))
                Library[name] = function;
            else
                Library.Add(name, function);
        }

        public bool Has(string name)
        {
            return Library.ContainsKey(name);
        }
    }
}
