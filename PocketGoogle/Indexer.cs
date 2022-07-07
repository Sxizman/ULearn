using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private IndexDictionary _dictionary;
        private Dictionary<int, string> _documents;

        public Indexer()
        {
            _dictionary = new IndexDictionary();
            _documents = new Dictionary<int, string>();
        }

        public void Add(int id, string documentText)
        {
            if (_documents.ContainsKey(id))
                throw new ArgumentException("Document with given id already exists");

            var wordsPositions = DocumentSplitter.SplitIntoWords(documentText);
            foreach (var wordPositions in wordsPositions)
                _dictionary.Add(wordPositions.Key, id, wordPositions.Value);
            _documents.Add(id, documentText);
        }

        public List<int> GetIds(string word)
        {
            return _dictionary.GetIds(word);
        }

        public List<int> GetPositions(int id, string word)
        {
            return _dictionary.GetPositions(id, word);
        }

        public void Remove(int id)
        {
            if (!_documents.ContainsKey(id))
                return;

            var wordsPositions = DocumentSplitter.SplitIntoWords(_documents[id]);
            foreach (var wordPositions in wordsPositions)
                _dictionary.Remove(wordPositions.Key, id);
            _documents.Remove(id);
        }
    }

    public static class DocumentSplitter
    {
        public static readonly char[] WordSeparators = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };

        public static Dictionary<string, List<int>> SplitIntoWords(string documentText)
        {
            var wordsPositions = new Dictionary<string, List<int>>();

            var index = 0;
            while (index < documentText.Length)
            {
                if (IsSeparator(documentText[index]))
                    index = SkipSeparators(documentText, index);
                else
                    index = AddWordPosition(wordsPositions, documentText, index);
            }

            return wordsPositions;
        }

        private static int AddWordPosition(Dictionary<string, List<int>> wordsPositions, string documentText, int position)
        {
            var wordLength = SkipWord(documentText, position) - position;
            var word = documentText.Substring(position, wordLength);
            if (wordsPositions.ContainsKey(word))
                wordsPositions[word].Add(position);
            else
                wordsPositions.Add(word, new List<int> { position });

            return position + wordLength;
        }

        private static int SkipWord(string documentText, int index)
        {
            while (index < documentText.Length && !IsSeparator(documentText[index]))
                ++index;
            return index;
        }

        private static int SkipSeparators(string documentText, int index)
        {
            while (index < documentText.Length && IsSeparator(documentText[index]))
                ++index;
            return index;
        }

        private static bool IsSeparator(char c)
        {
            return WordSeparators.Contains(c);
        }
    }

    public class IndexDictionary
    {
        private Dictionary<string, Dictionary<int, List<int>>> _words;

        public IndexDictionary()
        {
            _words = new Dictionary<string, Dictionary<int, List<int>>>();
        }

        public void Add(string word, int documentId, List<int> positions)
        {
            if (_words.ContainsKey(word))
                _words[word].Add(documentId, positions);
            else
                _words.Add(word, new Dictionary<int, List<int>>
                {
                    [documentId] = positions
                });
        }

        public void Remove(string word, int documentId)
        {
            if (!_words.ContainsKey(word))
                return;
            
            var indicies = _words[word];
            if (!indicies.ContainsKey(documentId))
                return;

            indicies.Remove(documentId);
            if (indicies.Count == 0)
                _words.Remove(word);
        }

        public List<int> GetIds(string word)
        {
            if (!_words.ContainsKey(word))
                return new List<int>();

            return _words[word].Select(indicies => indicies.Key).ToList();
        }

        public List<int> GetPositions(int id, string word)
        {
            if (!_words.ContainsKey(word))
                return new List<int>();

            var indicies = _words[word];
            if (!indicies.ContainsKey(id))
                return new List<int>();

            return indicies[id];
        }
    }
}