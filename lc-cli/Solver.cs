using lc_cli.DataTypes;
using lc_cli.Library;
using System.Dynamic;

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
                var lib = output.GetLibrary();

                foreach (var t in lib.CKeys)
                    Console.WriteLine(t.Key + ": " + t.Count);

                var reduction = SmartBetaReduce(output, dictionary);
                //var reduction = BetaReduce(output, dictionary);

                changed = reduction.changed;
                output = reduction.reducedSegment;
                output.RedundancyCheck();

                if (verbose && changed)
                {
                    //Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.Write("  ~>");
                    //output.Print();
                    //Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  ~>");
                    output.OldPrint();
                    Console.WriteLine();

                    //Console.WriteLine("Press Any Key");

                    //Console.ReadKey();
                }

                Console.WriteLine("Press Any Key");

                Console.ReadKey();
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

        public static (Segment reducedSegment, bool changed) SmartBetaReduce(Segment input, Dictionary dictionary)
        {
            // search for aplicable pairs
            int skips = 0;

            (Segment, Counter, int) bestSkip = (input, input.GetLibrary(), Int32.MaxValue);

            var changed = true;

            var exploring = true;

            while (/*exploring*/changed)
            {
                changed = false;

                var copySeg = input.Copy();

                var skipsC = 0;

                var results = OffsetBetaReduce(copySeg, dictionary, skips, ref skipsC);
                //exploring = results.changed;
                changed = results.changed;

                if (exploring && results.reducedSegment.ToString() != input.ToString())
                {
                    var library = results.reducedSegment.GetLibrary();

                    // Value total count
                    var tCount = 0;

                    foreach (var key in library.CKeys)
                    {
                        tCount += key.Count;
                    }

                    if (tCount < bestSkip.Item3)
                        bestSkip = (results.reducedSegment, library, tCount);
                }
                skips++;
            }

            // chose best
            if (input.GetLibrary().CKeys.Count - bestSkip.Item2.CKeys.Count > 1)
                Console.WriteLine("ohno?");

            return (bestSkip.Item1, bestSkip.Item3 != Int32.MaxValue);
        }

        public static (Segment reducedSegment, bool changed) OffsetBetaReduce(Segment input, Dictionary dictionary, int targetSkips, ref int skips)
        {
            var changed = false;

            var output = new Segment { Elements = new() };

            var layerFuncReached = false;

            for (int i = 0; i < input.Elements.Count; i++)
            {
                if (input.Elements[i].GetType() == typeof(Segment) && !changed)
                {
                    var result = OffsetBetaReduce((Segment)input.Elements[i], dictionary, targetSkips, ref skips);

                    output.Elements.Add(result.reducedSegment);
                    changed = result.changed;
                }
                else if (input.Elements[i].GetType() == typeof(Function) && !changed)
                {
                    var function = ((Function)input.Elements[i]).Copy();

                    if (i + 1 < input.Elements.Count && !layerFuncReached)
                    {
                        if (skips == targetSkips)
                        {
                            output.Elements.Add(function.ApplyElement(input.Elements[i + 1]));

                            i++;
                            changed = true;
                        }
                        else
                        {
                            layerFuncReached = true;
                            output.Elements.Add(function);
                            skips++;
                        }
                    }
                    else
                    {
                        var betaReducedSeg = OffsetBetaReduce((Segment)function.Body, dictionary, targetSkips, ref skips);
                        function.Body = betaReducedSeg.reducedSegment;

                        output.Elements.Add(function);
                        changed = betaReducedSeg.changed;
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
            return (output, skips == targetSkips);
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
