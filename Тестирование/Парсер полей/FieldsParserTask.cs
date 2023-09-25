using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("text ", new[] { "text" })]
        [TestCase("'a\\' b'", new[] { "a' b" })]
        [TestCase("''", new[] { "" })]
        [TestCase("'a'", new[] { "a" })]
        [TestCase(" hello    world ", new[] { "hello", "world" })]
        [TestCase("hello  \" \"", new[] { "hello", " " })]
        [TestCase("\'hello\' world", new[] { "hello", "world" })]
        [TestCase("\"\"", new[] { "" })]
        [TestCase("''\"bcd ef\"'x y'", new[] { "", "bcd ef", "x y" })]
        [TestCase("abc\"def", new[] { "abc", "def" })]
        [TestCase("\"a'b'c\" \'1\"2\"3\'", new[] { "a'b'c", "1\"2\"3" })]
        [TestCase("\'abc\\\\'", new[] { "abc\\" })]
        [TestCase(@"""abc ", new[] { "abc " })]
        [TestCase("", new string[0])]
        [TestCase("\"\\\"a\\\"\"", new[] { "\"a\"" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            Token token = new Token(line, 0, line.Length);
            var tokens = new List<Token>();
            int startIndex = 0;

            if (line == "")
                return new List<Token>();

            for (; startIndex < line.Length;)
            {
                if (line[startIndex] == ' ')
                {
                    startIndex++;
                    continue;
                }
                if (line[startIndex] == '\'' || line[startIndex] == '\"')
                {
                    token = ReadQuotedField(line, startIndex);
                    tokens.Add(token);
                }
                else
                {
                    token = ReadField(line, startIndex);
                    tokens.Add(token);
                }
                startIndex = token.GetIndexNextToToken();
            }
            return tokens;
        }

        private static Token ReadField(string line, int startIndex)
        {
            var builder = new StringBuilder();

            for (int i = startIndex; i < line.Length; i++)
            {
                if (line[i] == ' ')
                    break;
                if (line[i] != '\'' && line[i] != '\"')
                    builder.Append(line[i]);
                else
                    break;
            }

            line = builder.ToString();
            return new Token(line, startIndex, line.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}