using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private Dictionary<string, Dictionary<int, List<int>>> wordsPosition =
            new Dictionary<string, Dictionary<int, List<int>>>();

        private List<int> FindIndexWord(string[] array, string target)
        {
            var result = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == target)
                    result.Add(i);
            }
            return result;
        }

        public void Add(int id, string documentText)
        {
            var charSeparators = new[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
            var splitWords = SplitDocument(charSeparators, documentText);
            for (int i = 0; i < splitWords.Length; i++)
            {
                if (!wordsPosition.ContainsKey(splitWords[i]))
                {
                    var tempDic = new Dictionary<int, List<int>>();
                    tempDic.Add(id, FindIndexWord(splitWords, splitWords[i]));
                    wordsPosition.Add(splitWords[i], tempDic);
                }
                else if (wordsPosition.ContainsKey(splitWords[i]) && !wordsPosition[splitWords[i]].ContainsKey(id))
                    wordsPosition[splitWords[i]].Add(id, FindIndexWord(splitWords, splitWords[i]));
            }
        }

        static string[] SplitDocument(char[] separators, string input)
        {
            List<string> result = new List<string>();

            int startIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (separators.Contains(input[i]))
                {
                    if (i > startIndex)
                    {
                        result.Add(input.Substring(startIndex, i - startIndex));
                    }
                    result.Add(input[i].ToString());
                    startIndex = i + 1;
                }
            }

            if (startIndex < input.Length)
            {
                result.Add(input.Substring(startIndex));
            }
            string[] resultArray = result.ToArray();
            return resultArray;
        }

        public List<int> GetIds(string word)
        {
            var tempDic = new Dictionary<int, List<int>>();
            if (wordsPosition.ContainsKey(word))
            {
                tempDic = wordsPosition[word];
                return tempDic.Keys.ToList();
            }
            else return new List<int> { };
        }

        public List<int> GetPositions(int id, string word)
        {
            if (wordsPosition.ContainsKey(word))
                if (wordsPosition[word].ContainsKey(id))
                    return wordsPosition[word][id];
            return new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var item in wordsPosition.Keys)
            {
                wordsPosition[item].Remove(id);
            }
        }
    }
}
