using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FP
{
    public static class SummatorExtensions
    {
        public static IEnumerable<T> Repeat<T>(this Func<T> getValue)
        {
            while (true)
                yield return getValue();
        }

        public static IEnumerable<T> RepeatUnitNull<T>(this Func<T> getValue)
        {
            return getValue.Repeat().TakeWhile(value => value != null);
        }

        public static IEnumerable<T> Notify<T>(this IEnumerable<T> ts, Action<int> nofity)
        {
            var counter = 0;
            foreach (var t in ts)
            {
                counter++;
                nofity(counter);
                yield return t;
            }
        }

        public static int[] NextParsedRecord(this DataSource dataSource)
        {
            return dataSource.NextRecord()?
                .Select(part => Convert.ToInt32(part, 16))
                .ToArray();
        }

        public static string SumAndFormat(this ISumFormatter formatter, int[] input)
        {
            return formatter.Format(input, input.Sum());
        }

        public static void WriteAllLines(this IEnumerable<string> lines, string path)
        {
            File.WriteAllLines(path, lines);
        }
    }
}