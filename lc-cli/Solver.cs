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
            //var change = false;

            //var output = new Segment { Elements = new() };

            //for(int i = 0; i < input.Elements.Count; i++)
            //{
            //    if (input.Elements[i].GetType() == typeof(Function))
            //    {
            //        if (i + 1 < input.Elements.Count)
            //        {
            //            var appliedData = (input.Elements[i] as Function)!.ApplyElement(input.Elements[i + 1]);
            //            foreach (Element element in appliedData.Elements)
            //                output.Elements.Add(element);

            //            i = input.Elements.Count;
            //            change = true;
            //        }
            //    }
            //    else
            //    {
            //        output.Elements.Add(input.Elements[i]);
            //    }
            //}

            var changed = true;
            Segment output = input.Copy();

            while (changed)
            {
                var reduction = BetaReduce(output);

                changed = reduction.changed;
                output = reduction.reducedSegment;
                output.RedundancyCheck();

                if (verbose && changed)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("~>");
                    output.Print();
                    Console.WriteLine();
                }
            }

            return output;
        }

        public static (Segment reducedSegment, bool changed) BetaReduce(Segment input)
        {
            var changed = false;

            var output = new Segment { Elements = new() };

            for (int i = 0; i < input.Elements.Count; i++)
            {
                if (input.Elements[i].GetType() == typeof(Segment) && !changed)
                {
                    var result = BetaReduce((Segment)input.Elements[i]);

                    output.Elements.Add(result.reducedSegment);
                    changed = result.changed;
                }
                else if (input.Elements[i].GetType() == typeof(Function) && !changed)
                {
                    if (i + 1 < input.Elements.Count)
                    {
                        output.Elements.Add(((Function)input.Elements[i]).ApplyElement(input.Elements[i + 1]));

                        i++;
                        changed = true;
                    }
                    else
                    {
                        output.Elements.Add(input.Elements[i].Copy());
                    }
                }
                else
                {
                    output.Elements.Add(input.Elements[i].Copy());
                }
            }

            return (output, changed);
        }
    }
}
