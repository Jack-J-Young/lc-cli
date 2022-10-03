using lc_cli.DataTypes;
using lc_cli.Library;

namespace lc_cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //    var inputType = InputType.None;
            //    var output = String.Empty;

            //    switch (args[0]) {
            //        case "convert":
            //            inputType = InputType.ConvertFile;
            //            break;
            //    }

            //    foreach (var arg in args.Skip(1))
            //    {
            //        switch (arg)
            //        {
            //            case "-v":
            //                //verbose
            //                break;
            //            default:
            //                if (inputType == InputType.ConvertFile)
            //                    //output = Convertor.Convert(arg);
            //                break;
            //        }
            //    }

            //    Console.WriteLine(output);

            //var x = new Variable { Name = "x" };
            //var y = new Variable { Name = "y" };

            //var function = new Function
            //{
            //    Input = x,
            //    Body = new Segment
            //    {
            //        Elements = new List<Element>
            //        {
            //            x,
            //            y
            //        }
            //    }
            //};

            //var seg = new Segment
            //{
            //    Elements = new List<Element>
            //    {
            //        function,
            //        new Application
            //        {
            //            Data = new Segment
            //            {
            //                Elements = new List<Element>
            //                {
            //                    y,
            //                    y,
            //                    x
            //                }
            //            }
            //        }
            //    }
            //};
            //Dictionary dictionary = new Dictionary();
            //dictionary.Add("IF", "(^a.(^t.(^f.a t f)))");
            var dictionary = Loader.LoadFromDir(@"C:\Temp\library");

            while(true)
            {
                Console.Write("^> ");
                var input = Console.ReadLine();

                if (input.Contains('='))
                {
                    Convertor.ConvertToLibrary(input, @"C:\Temp\library", dictionary);
                }
                else
                {
                    var seg = Convertor.ConvertStringToLc(input);

                    //Console.WriteLine(Convertor.LcToString(seg));

                    Console.Write("    ");
                    seg.Print();
                    Console.WriteLine();

                    Segment solved = Solver.Solve(seg, true, dictionary)/*.Print()*/;
                    Console.WriteLine();

                    dictionary["LAST"] = solved.ToString();
                }
            }
        }
    }

    public enum InputType
    {
        None,
        ConvertFile,
    }
}