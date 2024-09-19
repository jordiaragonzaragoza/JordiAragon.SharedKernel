namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using Ardalis.GuardClauses;
    using NetArchTest.Rules;

    public static class Utils
    {
        public static string GetFailingTypes(TestResult result)
        {
            Guard.Against.Null(result, nameof(result));

            return result.FailingTypeNames != null ?
                string.Join(", ", result.FailingTypeNames) :
                string.Empty;
        }
    }
}