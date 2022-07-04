using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var totalCount = Math.Min(count, GetCountByPrefix(phrases, prefix));
            return phrases.Skip(index).Take(totalCount).ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var first = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var last = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - 1;
            return last - first + 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        private static Phrases phrases = new Phrases(
            new[]
            {
                "a",
                "aaa",
                "aabb",
                "abc",
                "abcc",
                "bac",
                "bca",
                "caab",
                "cab",
                "cccc"
            },
            new[] { "" },
            new[] { "" });

        [Test]
        public void TopByPrefix_IsEmpty_WhenZeroCount()
        {
            CollectionAssert.IsEmpty(AutocompleteTask.GetTopByPrefix(phrases, "abc", 0));
        }

        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            CollectionAssert.IsEmpty(AutocompleteTask.GetTopByPrefix(
                new Phrases(new string[] { }, new string[] { }, new string[] { }),
                "abc",
                10));
        }

        [TestCase(100, new string[]
        {
            "a  ",
            "aaa  ",
            "aabb  ",
            "abc  ",
            "abcc  ",
            "bac  ", "bca " +
            "",
            "caab  ",
            "cab  ",
            "cccc  "
        })]
        [TestCase(10, new string[]
        {
            "a  ",
            "aaa  ",
            "aabb  ",
            "abc  ",
            "abcc  ",
            "bac  ", "bca " +
            "",
            "caab  ",
            "cab  ",
            "cccc  "
        })]
        [TestCase(1, new string[] { "a  " })]
        public void TopByPrefix_WhenNoPrefix(int count, string[] expectedPhrases)
        {
            CollectionAssert.AreEquivalent(expectedPhrases, AutocompleteTask.GetTopByPrefix(phrases, "", count));
        }

        [TestCase("abc", 10, new string[] { "abc  ", "abcc  " })]
        [TestCase("abc", 1, new string[] { "abc  " })]
        [TestCase("aaa", 10, new string[] { "aaa  " })]
        [TestCase("cb", 10, new string[] { })]
        public void TopByPrefix(string prefix, int count, string[] expectedPhrases)
        {
            CollectionAssert.AreEquivalent(expectedPhrases, AutocompleteTask.GetTopByPrefix(phrases, prefix, count));
        }

        [Test]
        public void CountByPrefix_IsZero_WhenNoPhrases()
        {
            Assert.AreEqual(0, AutocompleteTask.GetCountByPrefix(
                new Phrases(new string[] { }, new string[] { }, new string[] { }),
                "abc"));
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenNoPrefix()
        {
            Assert.AreEqual(phrases.Count, AutocompleteTask.GetCountByPrefix(phrases, ""));
        }

        [TestCase("abc", 2)]
        [TestCase("aaa", 1)]
        [TestCase("cb", 0)]
        public void CountByPrefix_IsTotalCount(string prefix, int expectedCount)
        {
            Assert.AreEqual(expectedCount, AutocompleteTask.GetCountByPrefix(phrases, prefix));
        }

        [Test]
        public void FirstByPrefix_IsNull_WhenNoPhrases()
        {
            Assert.AreEqual(null, AutocompleteTask.FindFirstByPrefix(
                new Phrases(new string[] { }, new string[] { }, new string[] { }),
                "abc"));
        }

        [Test]
        public void FirstByPrefix_IsFirst_WhenNoPrefix()
        {
            Assert.AreEqual(phrases[0], AutocompleteTask.FindFirstByPrefix(phrases, ""));
        }

        [TestCase("abc", "abc  ")]
        [TestCase("aaa", "aaa  ")]
        [TestCase("cb", null)]
        public void FirstByPrefix(string prefix, string expectedValue)
        {
            Assert.AreEqual(expectedValue, AutocompleteTask.FindFirstByPrefix(phrases, prefix));
        }
    }
}
