using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public class Variable : Element
    {
        public string Name { get; set; }

        public int FunctionId { get; set; }

        public override Variable Copy()
        {
            return new Variable
            {
                Name = Name,
                FunctionId = FunctionId
            };
        }

        public override void Print()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Name + FunctionId);
        }

        public override void OldPrint()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(Name);
        }


        public override string ToString()
        {
            return Name + FunctionId;
        }
    }
}
