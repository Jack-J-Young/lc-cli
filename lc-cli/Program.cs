using lc_cli.DataTypes;

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

            var seg = Convertor.ConvertString(Console.ReadLine());

            //Console.WriteLine(Convertor.LcToString(seg));
            seg.Print();
            Console.WriteLine();

            Solver.Solve(seg, true)/*.Print()*/;
            Console.WriteLine();
        }
    }

    public enum InputType
    {
        None,
        ConvertFile,
    }
}