using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var biGramm = new Dictionary<string, Dictionary<string, int>>();
            var triGramm = new Dictionary<string, Dictionary<string, int>>();
            var resultGramm = new Dictionary<string, Dictionary<string, int>>();

            biGramm = MakeFrequencyBiGramm(text);
            triGramm = MakeFrequencyTriGramm(text);
            biGramm.ToList().ForEach(x => resultGramm.Add(x.Key, x.Value));
            triGramm.ToList().ForEach(x => resultGramm.Add(x.Key, x.Value));
            result = MakeResultDictionary(resultGramm);

            return result;
        }

        public static Dictionary<string, string> MakeResultDictionary(Dictionary<string, Dictionary<string, int>> resultGramm)
        {
            var result = new Dictionary<string, string>();
            foreach (var item in resultGramm)
            {
                string firstEl = item.Key;
                string secondEl = " ";
                var insideDict = item.Value;

                foreach (var value in insideDict)
                {
                    var arrayValue = new int[insideDict.Count];
                    arrayValue = insideDict.Values.ToArray();
                    int max = arrayValue.Max();
                    var listKeyDis = new List<string>();
                    foreach (var keys in insideDict)
                    {
                        if (keys.Value == max)
                            listKeyDis.Add(keys.Key);
                    }
                    secondEl = FindLexicographicallyLess(listKeyDis);
                    break;
                }
                result.Add(firstEl, secondEl);
            }
            return result;
        }

        public static Dictionary<string, Dictionary<string, int>> MakeFrequencyBiGramm(List<List<string>> text)
        {
            var biGramm = new Dictionary<string, Dictionary<string, int>>();
            for (int i = 0; i < text.Count; i++)
            {
                int count = 1;
                for (int j = 0; j < text[i].Count - 1; j++)
                {
                    var insideDict = new Dictionary<string, int>();
                    string outKeyWord = text[i][j];
                    string secondWord = text[i][j + 1];

                    if (!biGramm.ContainsKey(outKeyWord))
                        biGramm.Add(outKeyWord, insideDict);

                    if (text[i].Count > 1 && !biGramm[outKeyWord].ContainsKey(secondWord))
                        biGramm[outKeyWord].Add(secondWord, count);
                    else
                    {
                        int countFrequncy = biGramm[outKeyWord][secondWord];
                        countFrequncy++;
                        biGramm[outKeyWord][secondWord] = countFrequncy;
                    }
                }
            }
            return biGramm;
        }

        public static Dictionary<string, Dictionary<string, int>> MakeFrequencyTriGramm(List<List<string>> text)
        {
            var triGramm = new Dictionary<string, Dictionary<string, int>>();
            for (int i = 0; i < text.Count; i++)
            {
                int count = 1;
                for (int j = 0; j < text[i].Count - 2; j++)
                {
                    var insideDict = new Dictionary<string, int>();
                    string outKeyWord = text[i][j] + " " + text[i][j + 1];
                    string secondWord = text[i][j + 2];

                    if (!triGramm.ContainsKey(outKeyWord))
                        triGramm.Add(outKeyWord, insideDict);

                    if (text[i].Count > 1 && !triGramm[outKeyWord].ContainsKey(secondWord))
                        triGramm[outKeyWord].Add(secondWord, count);
                    else
                    {
                        int countFrequncy = triGramm[outKeyWord][secondWord];
                        countFrequncy++;
                        triGramm[outKeyWord][secondWord] = countFrequncy;
                    }
                }
            }
            return triGramm;
        }

        public static string FindLexicographicallyLess(List<string> listKey)
        {
            string secondEl = listKey[0];
            if (listKey.Count == 1)
                return secondEl = listKey[0];

            foreach (var item in listKey)
            {
                string firstWord = item;
                foreach (var key in listKey)
                {
                    string secondWord = key;
                    int countOut = string.CompareOrdinal(firstWord, secondWord);
                    int countIn = string.CompareOrdinal(firstWord, secondEl);
                    if (countOut < 0 && countIn < 0)
                        secondEl = firstWord;
                }
            }
            return secondEl;
        }
    }
}