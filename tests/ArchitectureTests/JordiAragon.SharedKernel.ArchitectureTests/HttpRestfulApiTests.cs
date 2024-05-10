namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using FluentAssertions;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi;
    using NetArchTest.Rules;
    using Xunit;

    public class HttpRestfulApiTests
    {
        [Fact]
        public void ControllerBase_Should_HaveDependencyOnMediatR()
        {
            // Arrange.
            var assembly = HttpRestfulApiAssemblyReference.Assembly;

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("BaseApiController")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}