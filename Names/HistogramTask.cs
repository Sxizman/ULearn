using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), 

                Enumerable.Range(1, 31).Select(i => i.ToString()).ToArray(),

                Enumerable.Range(1, 31)
                .Select(i => (i == 1) ? 0.0 :
                names.Count(nameData => nameData.Name == name && nameData.BirthDate.Day == i))
                .ToArray());
        }
    }
}