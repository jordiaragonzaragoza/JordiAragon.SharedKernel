namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using NetArchTest.Rules;

    public static class Utils
    {
        public static string GetFailingTypes(TestResult result)
        {
            return result.FailingTypeNames != null ?
                string.Join(", ", result.FailingTypeNames) :
                string.Empty;
        }
    }
}
