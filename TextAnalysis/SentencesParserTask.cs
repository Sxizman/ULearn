using System;
using System.Linq;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        private static char[] _sentenceSeparators = { '.', '!', '?', ':', ';', '(', ')' };

        public static List<List<string>> ParseSentences(string text)
        {
            return text
                .Split(_sentenceSeparators)
                .Select(sentence => ParseWords(sentence))
                .Where(sentence => sentence.Count > 0)
                .ToList();
        }

        public static List<string> ParseWords(string sentence)
        {
            var words = new List<string>();

            for (var i = 0; i < sentence.Length; ++i)
            {
                while (i < sentence.Length && !IsWordChar(sentence[i]))
                    ++i;

                var wordBegin = i;
                while (i < sentence.Length && IsWordChar(sentence[i]))
                    ++i;

                if (i > wordBegin)
                    words.Add(sentence.Substring(wordBegin, i - wordBegin).ToLower());
            }

            return words;
        }

        public static bool IsWordChar(char c)
        {
            return char.IsLetter(c) || c == '\'';
        }
    }
}