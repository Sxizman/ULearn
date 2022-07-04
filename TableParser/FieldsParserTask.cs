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

        [TestCase(@"", new string[] { })]
        [TestCase(@"hello", new[] { @"hello" })]
        [TestCase(@"hello world", new[] { @"hello", @"world" })]
        [TestCase(@"hello    world", new[] { @"hello", @"world" })]
        [TestCase(@" hello world ", new[] { @"hello", @"world" })]

        [TestCase(@"'\'escaped\''", new[] { @"'escaped'" })]
        [TestCase(@"""\""escaped\""""", new[] { @"""escaped""" })]
        [TestCase(@"'\\escaped'", new[] { @"\escaped" })]
        [TestCase(@"'escaped\\'", new[] { @"escaped\" })]

        [TestCase(@"''", new[] { @"" })]
        [TestCase(@"'quotes", new[] { @"quotes" })]
        [TestCase(@"'quo tes", new[] { @"quo tes" })]
        [TestCase(@"'quotes ", new[] { @"quotes " })]
        [TestCase(@"'""quotes""'", new[] { @"""quotes""" })]
        [TestCase(@"""'quotes'""", new[] { @"'quotes'" })]

        [TestCase(@"'nearby''quotes'", new[] { @"nearby", @"quotes" })]
        [TestCase(@"nearby'fields'", new[] { @"nearby", @"fields" })]
        [TestCase(@"'nearby'fields", new[] { @"nearby", @"fields" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            // Тело метода изменять не нужно
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();

            var index = NextTokenIndex(line, 0);
            while (index < line.Length)
                index = ProceedNextToken(tokens, line, index);

            return tokens;
        }

        private static int ProceedNextToken(List<Token> tokens, string line, int startIndex)
        {
            var token = line[startIndex] == '\'' || line[startIndex] == '"' ?
                    ReadQuotedField(line, startIndex) :
                    ReadField(line, startIndex);
            tokens.Add(token);

            return NextTokenIndex(line, token.GetIndexNextToToken());
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            var tokenLength = 0;
            for (var i = startIndex; i < line.Length; ++i)
            {
                if (line[i] == ' ' || line[i] == '\'' || line[i] == '"')
                    break;
                ++tokenLength;
            }

            return new Token(line.Substring(startIndex, tokenLength), startIndex, tokenLength);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        public static int NextTokenIndex(string line, int startIndex)
        {
            var index = startIndex;
            while (index < line.Length && line[index] == ' ')
                ++index;
            return index;
        }
    }
}