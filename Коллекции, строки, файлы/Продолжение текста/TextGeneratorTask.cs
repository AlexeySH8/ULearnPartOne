using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords,
            string phraseBeginning, int wordsCount)
        {
            var builder = new StringBuilder();
            builder.Append(phraseBeginning);

            for (int i = 0; i < wordsCount; i++)
            {
                var arrayWords = phraseBeginning.Split(' ');

                if (arrayWords.Length > 1)
                {
                    string keyWord = arrayWords[arrayWords.Length - 2] +
                        " " + arrayWords[arrayWords.Length - 1];

                    if (nextWords.ContainsKey(keyWord))
                        builder.Append(" " + nextWords[keyWord]);
                    else if (nextWords.ContainsKey(arrayWords[arrayWords.Length - 1]))
                        builder.Append(" " + nextWords[arrayWords[arrayWords.Length - 1]]);
                    else
                        break;
                }
                else
                {
                    if (nextWords.ContainsKey(phraseBeginning))
                        builder.Append(" " + nextWords[phraseBeginning]);
                    else
                        break;
                }
                phraseBeginning = builder.ToString();
            }
            return phraseBeginning;
        }
    }
}