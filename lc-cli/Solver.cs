using lc_cli.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli
{
    public class Solver
    {
        public static Segment Solve(Segment input, bool verbose)
        {
            var change = false;

            var output = new Segment { Elements = new() };

            for(int i = 0; i < input.Elements.Count; i++)
            {
                if (input.Elements[i].GetType() == typeof(Function) && i + 1 < input.Elements.Count)
                {
                    var appliedData = (input.Elements[i] as Function)!.ApplyElement(input.Elements[i + 1]);
                    foreach (Element element in appliedData.Elements)
                        output.Elements.Add(element);

                    i++;
                    change = true;
                }
                else
                {
                    output.Elements.Add(input.Elements[i]);
                }
            }

            if (verbose && change)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("~>");
                output.Print();
                Console.WriteLine();
            }
            return change ? Solve(output, verbose) : output;
        }
    }
}
