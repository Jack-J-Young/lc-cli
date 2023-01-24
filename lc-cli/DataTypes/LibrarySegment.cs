using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public class LibrarySegment : Element
    {
        public string Name { get; set; }

        public override Element Copy()
        {
            return new LibrarySegment { Name = Name };
        }

        public override void Print()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Name);
        }

        public override void OldPrint()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Name);
        }
    }
}
