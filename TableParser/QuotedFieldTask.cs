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
        [TestCase(@"''", 0, @"", 2)]
        [TestCase(@"'a'", 0, @"a", 3)]
        [TestCase(@"'\''", 0, @"'", 4)]
        [TestCase(@"'""'", 0, @"""", 3)]
        [TestCase(@"""'\\'""", 0, @"'\'", 6)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var tokenValueBuilder = new StringBuilder();
            var tokenLength = 1;

            var escaped = false;
            for (var i = startIndex + 1; i < line.Length; ++i)
            {
                ++tokenLength;
                if (HandleFieldCharacter(line[i], line[startIndex], tokenValueBuilder, ref escaped))
                    break;
            }

            return new Token(tokenValueBuilder.ToString(), startIndex, tokenLength);
        }

        private static bool HandleFieldCharacter(
            char c,
            char quoteCharacter,
            StringBuilder tokenValueBuilder,
            ref bool escaped)
        {
            if (escaped)
            {
                tokenValueBuilder.Append(c);
                escaped = false;
            }
            else
            {
                if (c == quoteCharacter)
                    return true;

                if (c == '\\')
                    escaped = true;
                else
                    tokenValueBuilder.Append(c);
            }
            return false;
        }
    }
}
