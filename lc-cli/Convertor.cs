using lc_cli.DataTypes;

namespace lc_cli
{
    public class Convertor
    {
        public static Segment ConvertString(string input)
        {
            var output = new Segment { Elements = new() };

            for(var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '^':
                        // Function

                        var pointIndex = input.IndexOf('.', i + 1);

                        output.Elements.Add(new Function
                        {
                            Input = input.Substring(i + 1, pointIndex - (i + 1)),
                            Body = ConvertString(input.Substring(pointIndex + 1)),
                        });

                        i = input.Length;
                        break;
                    case '(':
                        // Segment
                        var endIndex = FindEndParenthesis(input, i + 1);

                        var segmentText = input.Substring(i + 1, endIndex - (i + 1));

                        var segment = ConvertString(segmentText);

                        output.Elements.Add(segment.Elements.Count == 1 ? segment.Elements.First() : segment);

                        i = endIndex + 1;
                        break;
                    default:
                        // Variable
                        var nextSpaceIndex = input.IndexOf(' ', i);

                        if (nextSpaceIndex == -1)
                        {
                            output.Elements.Add(new Variable
                            {
                                Name = input.Substring(i),
                            });

                            i = input.Length;
                        }
                        else
                        {
                            output.Elements.Add(new Variable
                            {
                                Name = input.Substring(i, nextSpaceIndex - i),
                            });

                            i = nextSpaceIndex;
                        }
                        break;
                }
            }

            return output;
        }

        private static int FindEndParenthesis(string input, int open)
        {
            var opened = 1;

            for (var i = open; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(': opened++; break;
                    case ')': opened--; break;
                }

                if (opened == 0) return i;
            }

            return -1;
        }

        public static string LcToString(Segment segment)
        {
            var output = String.Empty;

            foreach (Element element in segment.Elements)
            {
                output += output.Length == 0 ? "" : " ";

                Type elementType = element.GetType();

                if (elementType == typeof(Variable))
                {
                    Variable variable = (Variable)element;

                    output += $"{variable.Name}";
                }

                if (elementType == typeof(Function))
                {
                    Function function = (Function)element;

                    output += $"(^{function.Input}.{LcToString(function.Body)})";
                }

                if (elementType == typeof(Segment))
                {
                    Segment tsegment = (Segment)element;

                    output += $"({LcToString(tsegment)})";
                }
            }

            return output;
        }
    }
}
