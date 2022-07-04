using System;
using System.Linq;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            return new HeatmapData(
                "Пример карты интенсивностей",

                Reshape(Enumerable.Range(0, 30 * 12)
                .Select(i =>
                {
                    int month = i % 12 + 1;
                    int day = i / 12 + 2;
                    return (double)names.Count(nameData => nameData.BirthDate.Month == month && nameData.BirthDate.Day == day);
                })
                .ToArray(), 30, 12),

                Enumerable.Range(2, 30).Select(i => i.ToString()).ToArray(),

                Enumerable.Range(1, 12).Select(i => i.ToString()).ToArray());
        }

        public static double[,] Reshape(double[] array, int rows, int columns)
        {
            var newArray = new double[rows, columns];
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < columns; ++j)
                    newArray[i, j] = array[i * columns + j];
            return newArray;
        }
    }
}