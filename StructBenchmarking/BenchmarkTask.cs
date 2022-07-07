using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();                   // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers();  // что Garbadge Collector вызовется в середине измерений
                                            // и как-то повлияет на них.

            task.Run(); // предварительный запуск для компиляции

            var timer = new Stopwatch();
            timer.Start();
            for (var i = 0; i < repetitionCount; ++i)
                task.Run();
            timer.Stop();

            return (double)timer.ElapsedMilliseconds / repetitionCount;
		}
	}

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var benchmark = new Benchmark();
            var stringConstructorTime =
                benchmark.MeasureDurationInMs(new StringConstructorTask(), 400000);
            var stringBuilderTime =
                benchmark.MeasureDurationInMs(new StringBuilderTask(), 30000);

            Assert.Less(stringConstructorTime, stringBuilderTime);
        }
    }

    public class StringConstructorTask : ITask
    {
        public void Run()
        {
            var createdString = new string('a', 10000);
        }
    }

    public class StringBuilderTask : ITask
    {
        public void Run()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 10000; ++i)
                stringBuilder.Append('a');
            var createdString = stringBuilder.ToString();
        }
    }
}