namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using FluentAssertions;
    using JordiAragon.SharedKernel.Application;
    using NetArchTest.Rules;
    using Xunit;

    public class ApplicationTests
    {
        [Fact]
        public void Handlers_Should_Have_DependencyOnArdalisResult()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .And().DoNotHaveNameEndingWith("EventHandler")
                .And().DoNotHaveNameEndingWith("NotificationHandler")
                .Should()
                .HaveDependencyOn("Ardalis.Result")
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}