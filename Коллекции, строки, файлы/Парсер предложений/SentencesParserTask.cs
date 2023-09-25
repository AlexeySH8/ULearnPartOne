using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var resultList = new List<List<string>>();
            var sentencesList = MakeSentancesList(text);

            for (int i = 0; i < sentencesList.Count; i++)
            {
                var wordsList = new List<string>();
                string sentence = MakeSentance(sentencesList[i]);
                string[] words = sentence.Split('_');

                foreach (string item in words)
                {
                    if (item.Length != 0)
                        wordsList.Add(item);
                }
                if (wordsList.Count != 0)
                    resultList.Add(wordsList);
            }
            return resultList;
        }

        public static string MakeSentance(string sentence)
        {
            foreach (var chr in sentence)
            {
                if (!char.IsLetter(chr) && chr != '\'')
                {
                    sentence = sentence.Replace(chr, '_');
                }
            }
            return sentence;
        }

        public static List<string> MakeSentancesList(string text)
        {
            var sentencesList = new List<string>();
            char[] delimiterChars = { '.', '?', '!', ';', ':', '(', ')' };
            string[] arraySentences = text.ToLower().Split(delimiterChars);

            foreach (string item in arraySentences)
            {
                sentencesList.Add(item);
            }
            return sentencesList;
        }
    }
}