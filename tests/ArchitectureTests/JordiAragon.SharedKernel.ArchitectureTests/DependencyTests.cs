namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using FluentAssertions;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Application;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages;
    using JordiAragon.SharedKernel.Contracts;
    using JordiAragon.SharedKernel.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts;
    using JordiAragon.SharedKernel.Infrastructure;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EventStore;
    using JordiAragon.SharedKernel.Presentation.WebApi;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;
    using NetArchTest.Rules;
    using Xunit;

    public class DependencyTests
    {
        private readonly string sharedKernelNamespace = SharedKernelAssemblyReference.Assembly.GetName().Name;
        private readonly string sharedKernelContractsNamespace = SharedKernelContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string domainNamespace = DomainAssemblyReference.Assembly.GetName().Name;
        private readonly string domainContractsNamespace = DomainContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string applicationNamespace = ApplicationAssemblyReference.Assembly.GetName().Name;
        private readonly string applicationContractsNamespace = ApplicationContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string applicationContractsIntegrationMessagesNamespace = ApplicationContractsIntegrationMessagesAssemblyReference.Assembly.GetName().Name;
        private readonly string infrastructureNamespace = InfrastructureAssemblyReference.Assembly.GetName().Name;
        private readonly string infrastructureEntityFrameworkNamespace = InfrastructureEntityFrameworkAssemblyReference.Assembly.GetName().Name;
        private readonly string infrastructureEventStoreNamespace = InfrastructureEventStoreAssemblyReference.Assembly.GetName().Name;
        private readonly string webApiNamespace = WebApiAssemblyReference.Assembly.GetName().Name;
        private readonly string webApiContractsNamespace = WebApiContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string[] allProjects;

        public DependencyTests()
        {
            this.allProjects = new[]
            {
                this.sharedKernelNamespace,
                this.sharedKernelContractsNamespace,
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };
        }

        [Fact]
        public void SharedKernelContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = SharedKernelContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.sharedKernelContractsNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void SharedKernel_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = SharedKernelAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.sharedKernelContractsNamespace)
                .Or()
                .HaveDependencyOn(this.sharedKernelNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = DomainContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.domainNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.domainContractsNamespace)
                .Or()
                .HaveDependencyOn(this.sharedKernelContractsNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = DomainAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var dependencies = new[]
            {
                this.sharedKernelNamespace,
                this.domainContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.domainNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApplicationContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationContractsAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.sharedKernelNamespace,
                this.domainNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsIntegrationMessagesNamespace,
                this.domainContractsNamespace,
                this.sharedKernelContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.applicationContractsNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApplicationContractsIntegrationMessages_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationContractsIntegrationMessagesAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.sharedKernelContractsNamespace,
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.applicationContractsIntegrationMessagesNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var otherProjects = new[]
            {
                this.applicationContractsNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.applicationNamespace)
                .Or()
                .HaveDependencyOnAny(otherProjects)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.domainNamespace,
                this.applicationNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.applicationContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.infrastructureNamespace)
                .Or()
                .HaveDependencyOnAny(otherProjects)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void InfrastructureEntityFramework_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEntityFrameworkAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .And()
                .HaveDependencyOn(this.infrastructureEntityFrameworkNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void InfrastructureEventStore_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEventStoreAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .And()
                .HaveDependencyOn(this.infrastructureEventStoreNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void WebApiContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = WebApiContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.sharedKernelContractsNamespace,
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.webApiContractsNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void WebApi_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = WebApiAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.webApiNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}