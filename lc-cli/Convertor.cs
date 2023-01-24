using lc_cli.DataTypes;
using lc_cli.Library;
using System.Reflection.Metadata.Ecma335;

namespace lc_cli
{
    public class Convertor
    {
        public static Segment ConvertStringToLc(string input, List<Variable>? library = null, int nestedfunctionId = 1)
        {
            if (library == null) library = new();

            var output = new Segment { Elements = new() };

            for(var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        // Segment
                        var endIndex = FindEndParenthesis(input, i + 1);

                        if(input[i + 1] == '^')
                        {
                            // Function

                            var pointIndex = input.IndexOf('.', i + 2);

                            List<Variable> arguments = new();

                            foreach (char c in input.Substring(i + 2, pointIndex - (i + 2)))
                            {
                                arguments.Add(new Variable { FunctionId = nestedfunctionId, Name = c + "" });
                            }

                            var newLibrary = new List<Variable>(library);

                            newLibrary.AddRange(arguments);

                            var bodyString = input.Substring(pointIndex + 1, endIndex - (pointIndex + 1));

                            output.Elements.Add(new Function
                            {
                                Inputs = arguments,
                                Body = ConvertStringToLc(bodyString, newLibrary, nestedfunctionId*10),
                            });
                        }
                        else
                        {
                            var segmentText = input.Substring(i + 1, endIndex - (i + 1));

                            var segment = ConvertStringToLc(segmentText, library, nestedfunctionId);

                            output.Elements.Add(segment.Elements.Count == 1 ? segment.Elements.First() : segment);
                        }
                        i = endIndex;

                        break;
                    default:
                        if (/*input[i] >= 97 && input[i] <= 122*/ true)
                        {
                            // Variable

                            var match = library.Where(x => x.Name == "" + input[i]).MaxBy(x => x.FunctionId);

                            if (match != null)
                            {
                                output.Elements.Add(match);
                            }
                            else
                            {
                                output.Elements.Add(new Variable
                                {
                                    Name = input[i] + "",
                                    FunctionId = nestedfunctionId
                                });
                            }
                        }
                        else
                        {
                            // Library Function
                            var nextSpaceIndex = input.IndexOf(' ', i);

                            if (nextSpaceIndex == -1)
                            {
                                output.Elements.Add(new LibrarySegment
                                {
                                    Name = input.Substring(i),
                                });

                                i = input.Length;
                            }
                            else
                            {
                                output.Elements.Add(new LibrarySegment
                                {
                                    Name = input.Substring(i, nextSpaceIndex - i),
                                });

                                i = nextSpaceIndex;
                            }
                        }
                        break;
                }
                nestedfunctionId++;
            }

            return output;
        }

        public static void ConvertToLibrary(string input, string libraryPath, Dictionary dictionary)
        {
            var inputSplit = input.Split('=');

            var name = inputSplit[0];
            var body = inputSplit[1];

            //if (dictionary has name)
            dictionary.Add(name, body);
            File.WriteAllText($"{libraryPath}\\{name}.lcf", body);
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
    }
}
