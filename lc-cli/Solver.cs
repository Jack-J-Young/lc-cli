using lc_cli.DataTypes;
using lc_cli.Library;

namespace lc_cli
{
    public class Solver
    {
        public static Segment Solve(Segment input, bool verbose, Dictionary dictionary)
        {
            input = ApplyLibrarySegments(input, verbose, dictionary);
            Console.WriteLine();

            var changed = true;
            Segment output = input.Copy();

            while (changed)
            {
                var reduction = BetaReduce(output, dictionary);

                changed = reduction.changed;
                output = reduction.reducedSegment;
                output.RedundancyCheck();

                if (verbose && changed)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  ~>");
                    output.Print();
                    Console.WriteLine();
                }
            }

            return output;
        }

        public static Segment ApplyLibrarySegments(Segment input, bool verbose, Dictionary dictionary)
        {
            var changed = true;
            Segment output = input.Copy();

            while (changed)
            {
                var reduction = ApplyLibraryAtomicly(output, dictionary);

                changed = reduction.changed;
                output = reduction.reducedSegment;
                output.RedundancyCheck();

                if (verbose && changed)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  ~>");
                    output.Print();
                    Console.WriteLine();
                }
            }

            return output;
        }

        public static (Segment reducedSegment, bool changed) BetaReduce(Segment input, Dictionary dictionary)
        {
            var changed = false;

            var output = new Segment { Elements = new() };

            for (int i = 0; i < input.Elements.Count; i++)
            {
                if (input.Elements[i].GetType() == typeof(Segment) && !changed)
                {
                    var result = BetaReduce((Segment)input.Elements[i], dictionary);

                    output.Elements.Add(result.reducedSegment);
                    changed = result.changed;
                }
                else if (input.Elements[i].GetType() == typeof(Function) && !changed)
                {
                    var function = ((Function)input.Elements[i]).Copy();

                    if (i + 1 < input.Elements.Count)
                    {
                        output.Elements.Add(function.ApplyElement(input.Elements[i + 1]));

                        i++;
                        changed = true;
                    }
                    else
                    {
                        if (function.Body.GetType() == typeof(Segment))
                        {
                            var betaReducedSeg = BetaReduce((Segment)function.Body, dictionary);
                            function.Body = betaReducedSeg.reducedSegment;

                            output.Elements.Add(function);
                            changed = betaReducedSeg.changed;
                        }
                        else
                            output.Elements.Add(function);
                    }
                }
                //else if (input.Elements[i].GetType() == typeof(LibrarySegment) && !changed)
                //{
                //    output.Elements.Add(Convertor.ConvertStringToLc(dictionary[((LibrarySegment)input.Elements[i].Copy()).Name]));

                //    changed = true;
                //}
                else
                {
                    output.Elements.Add(input.Elements[i].Copy());
                }
            }

            return (output, changed);
        }

        public static (Segment reducedSegment, bool changed) ApplyLibraryAtomicly(Segment input, Dictionary dictionary)
        {
            var changed = false;

            var output = new Segment { Elements = new() };

            for (int i = 0; i < input.Elements.Count; i++)
            {
                if (input.Elements[i].GetType() == typeof(Segment) && !changed)
                {
                    var result = ApplyLibraryAtomicly((Segment)input.Elements[i], dictionary);

                    output.Elements.Add(result.reducedSegment);
                    changed = result.changed;
                }
                else if (input.Elements[i].GetType() == typeof(Function) && !changed)
                {
                    var function = ((Function)input.Elements[i]).Copy();

                    var betaReducedSeg = ApplyLibraryAtomicly((Segment)function.Body, dictionary);
                    function.Body = betaReducedSeg.reducedSegment;

                    output.Elements.Add(function);
                    changed = betaReducedSeg.changed;
                }
                else if (input.Elements[i].GetType() == typeof(LibrarySegment) && !changed)
                {
                    output.Elements.Add(Convertor.ConvertStringToLc(dictionary[((LibrarySegment)input.Elements[i].Copy()).Name]));

                    changed = true;
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
