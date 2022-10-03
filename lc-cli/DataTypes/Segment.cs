using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public class Segment : Element
    {
        public List<Element> Elements { get; set; }

        public void aPrint()
        {
            bool lastWasVar = false;

            foreach (Element element in this.Elements)
            {

                Type elementType = element.GetType();

                if (elementType == typeof(Variable))
                {
                    Variable variable = (Variable)element;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write((lastWasVar ? " " : "") + variable.Name);

                    lastWasVar = true;
                }

                if (elementType == typeof(Function))
                {
                    Function function = (Function)element;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("(^");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(function.Input);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(".");

                    
                    function.Body.Print();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(")");

                    lastWasVar = true;
                }

                if (elementType == typeof(Segment))
                {
                    Segment segment = (Segment)element;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write((lastWasVar ? " " : "") + "(");

                    segment.Print();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(")");

                    lastWasVar = false;
                }
            }
        }

        public override Element RedundancyCheck()
        {
            for (var i = 0; i < Elements.Count; i++) Elements[i] = Elements[i].RedundancyCheck();

            return Elements.Count == 1 ? Elements[0] : this;
        }

        public override Segment Copy()
        {
            var copiedElements = new List<Element>();

            foreach (Element element in Elements) copiedElements.Add(element.Copy());

            return new Segment
            {
                Elements = copiedElements
            };
        }

        public override void Print()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("(");

            var start = true;
            foreach(Element element in Elements)
            {
                if (!start)
                    Console.Write(' ');

                element.Print();

                start = false;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(")");
        }

        public override string ToString()
        {
            var contents = String.Empty;

            var start = true;
            foreach (Element element in Elements)
            {
                contents += (start ? "" : ' ') + element.ToString();

                start = false;
            }

            return $"({contents})";
        }
    }
}
