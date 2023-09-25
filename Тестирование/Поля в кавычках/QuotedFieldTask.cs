using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'a\\' b'", 0, "a' b", 7)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }

        // Добавьте свои тесты
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            char startChar = line[startIndex];
            var builder = new StringBuilder();
            int resultLength = 1;

            for (int i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i] == '\\')
                {
                    builder.Append(line[i + 1]);
                    resultLength += 2;
                    i++;
                    continue;
                }
                if (line[i] != startChar)
                {
                    builder.Append(line[i]);
                    resultLength++;
                }
                else if (line[i] == startChar)
                {
                    resultLength++;
                    break;
                }
                else break;
            }

            line = builder.ToString();
            return new Token(line, startIndex, resultLength);
        }
    }
}
