using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            return BuildChartDataForTask(
                benchmark,
                new ArrayCreationTaskCreator(),
                repetitionsCount,
                "Create array");
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            return BuildChartDataForTask(
                benchmark,
                new MethodCallTaskCreator(),
                repetitionsCount,
                "Call method with argument");
        }

        public static ChartData BuildChartDataForTask(
            IBenchmark benchmark,
            IExperimentTaskCreator taskCreator,
            int repetitionsCount,
            string chartTitle)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var fieldCount in Constants.FieldCounts)
            {
                var classTask = taskCreator.CreateClassTask(fieldCount);
                classesTimes.Add(new ExperimentResult(fieldCount,
                    benchmark.MeasureDurationInMs(classTask, repetitionsCount)));

                var structTask = taskCreator.CreateStructTask(fieldCount);
                structuresTimes.Add(new ExperimentResult(fieldCount,
                    benchmark.MeasureDurationInMs(structTask, repetitionsCount)));
            }

            return new ChartData
            {
                Title = chartTitle,
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }

    public interface IExperimentTaskCreator
    {
        ITask CreateClassTask(int size);
        ITask CreateStructTask(int size);
    }

    public class ArrayCreationTaskCreator : IExperimentTaskCreator
    {
        public ITask CreateClassTask(int size)
        {
            return new ClassArrayCreationTask(size);
        }

        public ITask CreateStructTask(int size)
        {
            return new StructArrayCreationTask(size);
        }
    }

    public class MethodCallTaskCreator : IExperimentTaskCreator
    {
        public ITask CreateClassTask(int size)
        {
            return new MethodCallWithClassArgumentTask(size);
        }

        public ITask CreateStructTask(int size)
        {
            return new MethodCallWithStructArgumentTask(size);
        }
    }
}