using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            return new NGramm[] { }
                .Concat(GetAllNGramms(text, 2))
                .Concat(GetAllNGramms(text, 3))
                .GroupBy(ngramm => ngramm.Begin,
                         ngramm => ngramm.Continuation,
                         (begin, continuations) => new KeyValuePair<string, string>(
                             begin,
                             GetMostFrequentWord(continuations)))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static string GetMostFrequentWord(IEnumerable<string> words)
        {
            return words
                .GroupBy(word => word)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key, StringComparer.Ordinal)
                .First()
                .First();
        }

        private static List<NGramm> GetAllNGramms(List<List<string>> text, int n)
        {
            var ngramms = new List<NGramm>();
            for (var i = 0; i < text.Count; ++i)
            {
                var sentence = text[i].ToArray();
                for (var j = n - 1; j < sentence.Length; ++j)
                    ngramms.Add(new NGramm
                    {
                        Begin = string.Join(" ", sentence, j - (n - 1), n - 1),
                        Continuation = sentence[j]
                    });
            }
            return ngramms;
        }

        private class NGramm
        {
            public string Begin { get; set; }
            public string Continuation { get; set; }
        }
    }
}