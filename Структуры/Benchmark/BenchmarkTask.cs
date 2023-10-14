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
            Stopwatch stopwatch = new Stopwatch();
            double measurementsSum = 0;

            for (int i = 0; i < repetitionCount + 1; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                stopwatch.Restart();
                task.Run();
                stopwatch.Stop();
                if (i > 0)
                    measurementsSum += stopwatch.Elapsed.TotalMilliseconds;
            }

            return measurementsSum / repetitionCount;
        }
    }

    public class StringBuilderTest : ITask
    {
        public void Run()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
                builder.Append('a');

            builder.ToString();
        }
    }

    public class StringTest : ITask
    {
        public void Run()
        {
            new string('a', 10000);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var testBuilder = new StringBuilderTest();
            var testString = new StringTest();
            var benchmark = new Benchmark();

            var test1 = benchmark.MeasureDurationInMs(testBuilder, 10000);
            var test2 = benchmark.MeasureDurationInMs(testString, 10000);
            Assert.Less(test2, test1);
        }
    }
}