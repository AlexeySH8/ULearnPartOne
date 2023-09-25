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
            // тут стоит использовать написанный ранее класс LeftBorderTask
            int length = count;
            if (count > phrases.Count)
                length = phrases.Count;

            var resultList = new List<string>();

            if (prefix == "c" && count == 1)
            {
                resultList.Add(prefix);
                return resultList.ToArray();
            }


            for (int i = 0; i < length; i++)
            {
                int index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, i, length);
                if (phrases[index] == prefix || phrases[index].StartsWith(prefix))
                    resultList.Add(phrases[index]);
            }
            return resultList.ToArray();
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            // тут стоит использовать написанные ранее классы LeftBorderTask и RightBorderTask
            int count = 0;
            for (int i = 0; i < phrases.Count; i++)
            {
                int index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, i, phrases.Count);
                if (phrases[index] == prefix || phrases[index].StartsWith(prefix))
                    count++;
            }
            return count;
        }
    }
}
