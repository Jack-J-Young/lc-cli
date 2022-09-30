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

        public Segment Body { get; set; }

        public Segment ApplyElement(Element element)
        {
            var output = new Segment { Elements = new() };

            foreach (Element bElement in Body.Elements)
            {
                if (bElement.GetType() == typeof(Variable))
                {
                    output.Elements.Add((bElement as Variable)!.Name == Input ? element.Copy() : bElement);
                }
                else if (bElement.GetType() == typeof(Segment))
                {
                    var tempFunction = new Function
                    {
                        Input = Input,
                        Body = (bElement as Segment)!.Copy(),
                    };

                    output.Elements.Add(tempFunction.ApplyElement(element));
                }
                else if (bElement.GetType() == typeof(Function))
                {
                    var function = (bElement as Function)!.Copy();

                    if (function.Input != Input)
                    {
                        var tempFunction = new Function
                        {
                            Input = Input,
                            Body = function.Body.Copy(),
                        };

                        function.Body = tempFunction.ApplyElement(element);
                    }
                    
                    output.Elements.Add(function);
                }
            }

            return output;
        }

        public override Function Copy()
        {
            return new Function
            {
                Input = Input,
                Body = Body.Copy()
            };
        }
    }
}
