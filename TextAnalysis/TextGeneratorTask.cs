using System.Linq;
using System.Collections.Generic;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var words = SentencesParserTask.ParseSentences(phraseBeginning).Last();
            for (var i = 0; i < wordsCount; ++i)
                if (!TryContinuePhrase(nextWords, words))
                    break;
            return string.Join(" ", words);
        }

        public static bool TryContinuePhrase(Dictionary<string, string> nextWords, List<string> words)
        {
            var continuation = FindNGrammContinuation(nextWords, words, 3) ??
                               FindNGrammContinuation(nextWords, words, 2);
            if (continuation is null)
                return false;

            words.Add(continuation);
            return true;
        }

        public static string FindNGrammContinuation(Dictionary<string, string> nextWords, List<string> words, int n)
        {
            if (words.Count < n - 1)
                return null;

            var key = string.Join(" ", words.ToArray(), words.Count - (n - 1), n - 1);
            if (!nextWords.ContainsKey(key))
                return null;

            return nextWords[key];
        }
    }
}