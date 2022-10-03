using lc_cli.DataTypes;
using lc_cli.Library;

namespace lc_cli
{
    public class Convertor
    {
        public static Segment ConvertStringToLc(string input)
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
                            Body = ConvertStringToLc(input.Substring(pointIndex + 1)),
                        });

                        i = input.Length;
                        break;
                    case '(':
                        // Segment
                        var endIndex = FindEndParenthesis(input, i + 1);

                        var segmentText = input.Substring(i + 1, endIndex - (i + 1));

                        var segment = ConvertStringToLc(segmentText);

                        output.Elements.Add(segment.Elements.Count == 1 ? segment.Elements.First() : segment);

                        i = endIndex + 1;
                        break;
                    default:
                        if (input[i] >= 97 && input[i] <= 122)
                        {
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
