using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public class Function : Element
    {
        public List<Variable> Inputs { get; set; }

        public Segment Body { get; set; }

        public Element ApplyElement(Element element)
        {
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.Write("F> ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.Write(Input);
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.Write("|");
            //Body.Print();
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.Write("|");
            //element.Print();
            //Console.WriteLine();

            var first = Inputs.First();

            var applied = ApplyElementAtomicly(first.Name + first.FunctionId, Body, element);

            if (Inputs.Count == 1)
                return applied;
            else
            {
                var copy = this.Copy();
                copy.Inputs.RemoveAt(0);
                if (applied.GetType() == typeof(Segment))
                    copy.Body = (Segment)applied;
                else
                    copy.Body = new Segment{ Elements = { applied } };

                return copy;
            }
        }

        public static Element ApplyElementAtomicly(string argument, Element body, Element element)
        {
            Type bodyType = body.GetType();

            if (bodyType == typeof(Variable))
            {
                var cast = (Variable)body;

                return argument == cast.Name + cast.FunctionId ? element : body;
            }
            else if (bodyType == typeof(Segment))
            {
                var oldSegment = ((Segment)body).Copy();

                for (var i = 0; i < oldSegment.Elements.Count; i++)
                {
                    oldSegment.Elements[i] = ApplyElementAtomicly(argument, oldSegment.Elements[i], element);
                }

                return oldSegment;
            }
            else if (bodyType == typeof(Function))
            {
                var oldFunction = ((Function)body).Copy();

                oldFunction.Body.Elements = new List<Element> { ApplyElementAtomicly(argument, oldFunction.Body.Copy(), element) };

                return oldFunction;
            }
            else
            {
                return body;
            }
        }

        public override Element RedundancyCheck()
        {
            Body.RedundancyCheck();

            return this;
        }

        public override Function Copy()
        {
            var copyinputs = new List<Variable>();

            foreach(var a in Inputs)
                copyinputs.Add(a.Copy());

            return new Function
            {
                Inputs = copyinputs,
                Body = Body.Copy()
            };
        }

        public override void Print()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("(^");

            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var input in Inputs)
                Console.Write(input.Name + input.FunctionId);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(".");

            Body.Print();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(")");
        }

        public override void OldPrint()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("(^");

            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var input in Inputs)
                Console.Write(input.Name);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(".");

            Body.OldPrint();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(")");
        }

        public override string ToString()
        {
            var c = "";

            foreach (var input in Inputs)
               c += input.Name + input.FunctionId;

            return $"(^{c}.{Body.ToString()})";
        }
    }
}
