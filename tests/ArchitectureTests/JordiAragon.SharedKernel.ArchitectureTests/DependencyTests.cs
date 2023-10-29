namespace JordiAragon.SharedKernel.ArchitectureTests
{
    using System.Linq;
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
            var types = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Where(t => !t.Namespace.Contains(this.sharedKernelContractsNamespace));

            // Assert.
            types.Should().BeEmpty();
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
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void SharedKernel_Should_HaveDependencyOnSharedKernelContractsProject()
        {
            // Arrange.
            var assembly = SharedKernelAssemblyReference.Assembly;

            // Act.
            var types = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOn(this.sharedKernelContractsNamespace)
                .GetTypes();

            // Assert.
            types.Should().NotBeEmpty();
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
            var types = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Where(t => !t.Namespace.Contains(this.domainContractsNamespace));

            // Assert.
            types.Should().BeEmpty();
        }

        [Fact]
        public void DomainContracts_Should_HaveDependencyOnSharedKernelContractsProject()
        {
            // Arrange.
            var assembly = DomainContractsAssemblyReference.Assembly;

            // Act.
            var types = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOn(this.sharedKernelContractsNamespace)
                .GetTypes();

            // Assert.
            types.Should().NotBeEmpty();
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = DomainAssemblyReference.Assembly;

            var otherProjects = new[]
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

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Domain_Should_HaveSomeDependencies()
        {
            // Arrange.
            var assembly = DomainAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.domainContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Any();

            // Assert.
            testResult.Should().BeTrue();
        }

        [Fact]
        public void ApplicationContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationContractsAssemblyReference.Assembly;

            var otherProjects = new[]
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

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Where(t => !t.Namespace.Contains(this.applicationContractsNamespace));

            // Assert.
            testResult.Should().BeEmpty();
        }

        [Fact]
        public void ApplicationContracts_Should_HaveSomeDependencies()
        {
            // Arrange.
            var assembly = ApplicationContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationContractsIntegrationMessagesNamespace,
                this.domainContractsNamespace,
            };

            // Act.
            var types = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Any();

            // Assert.
            types.Should().BeTrue();
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
            var types = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Where(t => !t.Namespace.Contains(this.applicationContractsIntegrationMessagesNamespace));

            // Assert.
            types.Should().BeEmpty();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Application_Should_HaveSomeDependencies()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationContractsNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Any();

            // Assert.
            testResult.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Infrastructure_Should_HaveSomeDependencies()
        {
            var assembly = InfrastructureAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Any();

            // Assert.
            testResult.Should().BeTrue();
        }

        [Fact]
        public void InfrastructureEntityFramework_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEntityFrameworkAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void InfrastructureEntityFramework_Should_HaveSomeDependencies()
        {
            var assembly = InfrastructureEntityFrameworkAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Any();

            // Assert.
            testResult.Should().BeTrue();
        }

        [Fact]
        public void InfrastructureEventStore_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEventStoreAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
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
            var types = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetTypes()
                .Where(t => !t.Namespace.Contains(this.webApiContractsNamespace));

            // Assert.
            types.Should().BeEmpty();
        }
    }
}