using Xunit.Abstractions;
using Xunit.Sdk;

namespace FC.BuildingBlocks.Integration.Tests
{
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; }
        public TestPriorityAttribute(int priority) => Priority = priority;
    }

    public class AlphabeticalOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sortedMethods = testCases
                .Select(tc => new
                {
                    TestCase = tc,
                    Priority = tc.TestMethod.Method
                        .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
                        .FirstOrDefault()?.GetNamedArgument<int>("Priority") ?? 0
                })
                .OrderBy(x => x.Priority)
                .Select(x => x.TestCase);

            return sortedMethods;
        }
    }
}
