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
        public string Input { get; set; }

        public Element Body { get; set; }

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

            var applied = ApplyElementAtomicly(Input, Body, element);

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.Write("A> ");
            //applied.Print();
            //Console.WriteLine();

            return applied;
        }

        public static Element ApplyElementAtomicly(string argument, Element body, Element element)
        {
            Type bodyType = body.GetType();

            if (bodyType == typeof(Variable))
            {
                return argument == ((Variable)body).Name ? element : body;
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

                oldFunction.Body = ApplyElementAtomicly(argument, oldFunction.Body.Copy(), element);

                return oldFunction;
            } else
            {
                return body;
            }
        }

        public override Element RedundancyCheck()
        {
            Body = Body.RedundancyCheck();

            return this;
        }

        public override Function Copy()
        {
            return new Function
            {
                Input = Input,
                Body = Body.Copy()
            };
        }

        public override void Print()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("(^");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(Input);

            Body.Print();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(")");
        }
    }
}
