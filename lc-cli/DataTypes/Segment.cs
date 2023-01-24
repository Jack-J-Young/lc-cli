using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.DataTypes
{
    public class Counter
    {
        public List<CKeys> CKeys { get; set; } = new();

        public void AddOrInc(string name, int ammount = 1)
        {
            var matches = CKeys.Where(x => x.Key == name);

            if (matches.Any())
                foreach (var ckey in matches)
                    ckey.Count += ammount;
            else
                CKeys.Add(new CKeys { Key = name, Count = ammount});
        }

        public void Merge(Counter c)
        {
            foreach (var k in c.CKeys)
                AddOrInc(k.Key, k.Count);
        }
    }

    public class CKeys
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }

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
                    Console.Write(function.Inputs);

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

        //public void ApplyFunction(int index)
        //{
        //    if (Elements[index].GetType() == typeof(Function) && index + 1 < Elements.Count)
        //    {
        //        var applied = ((Function)Elements[index]).ApplyElement(Elements[index + 1]);

        //        Elements.Insert(index, applied);
        //        Elements.RemoveAt(index + 1);
        //        Elements.RemoveAt(index + 2);
        //    }
        //}

        public Counter GetLibrary()
        {
            Counter library = new();

            foreach(Element e in Elements)
            {
                if (e.GetType() == typeof(Segment))
                {
                    var sub = ((Segment)e).GetLibrary();

                    library.Merge(sub);

                }
                else if (e.GetType() == typeof(Function))
                {
                    foreach(Variable v in ((Function)e).Inputs)
                    {
                        library.AddOrInc(v.Name + v.FunctionId);
                    }

                    var sub = ((Function)e).Body.GetLibrary();

                    library.Merge(sub);
                }
                else
                {
                    library.AddOrInc(((Variable)e).Name + ((Variable)e).FunctionId);
                }
            }

            return library;
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
            foreach (Element element in Elements)
            {
                if (!start)
                    Console.Write(' ');

                element.Print();

                start = false;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(")");
        }

        public override void OldPrint()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("(");

            var start = true;
            foreach (Element element in Elements)
            {
                if (!start)
                    Console.Write(' ');

                element.OldPrint();

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
