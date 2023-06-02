What is the SharedKernel Project?
=====================
The SharedKernel Project is a project written in .NET 7.

The goal of this project is create the building blocks to follow DDD principles and Clean Architecture. 

## How to use:
- You will need the latest Visual Studio 2022 and the latest .NET Core SDK.
- All the projects library are contained in JordiAragon.SharedKernel.sln solution file.

## Used libraries:

- MediatR
- AutoMapper
- Autofac
- MassTransit
- Ardalis Libraries
 - Ardalis.Result
 - Ardalis.Specification
 - Ardalis.SmartEnums
 - Ardalis.GuardClauses
- FluentValidator
- Serilog
- Quartz
- Refit
- EasyCaching
- Volo.Abp.Guids Generator
- StyleCop & SonarAnalyzer

## Brief explanation per project (Pending to complete)

**SharedKernel and Contracts**
- Dependency Injection. Allow auto register services using markup interfaces.

**Domain and Contracts**
- Common contracts and base implementations to flow DDD principles like: DomainEvents, ApplicationEvents, ValueObjects, Entities, Repository, BusinessRulesValidations and other DDD building blocks.

**Application and Contracts**
- Application common contracts and base implementation for EventBus, UnitOfWork and CQRS...
- Common MediatR pipelines implementation 
- IntegrationMessages contacts and base implementation to be used in EventBus
- Common EventBus implementation using MassTransit

**Infrastructure and Contracts**
- EntityFramework base implementation to allow auditing and outbox to dispatch domain events notifications. 
- BaseCachedRepository is done using ICacheService abstraction.
- MassTransitEventBus base implementation.

**Presentation and Contracts**
- WebApi building blocks. Like BaseApiController, ExceptionMiddleware and CurrentUserService

## Resources and Inspiration

- <a href="https://github.com/ardalis/CleanArchitecture" target="_blank">Ardalis: Clean Architecture</a>
- <a href="https://www.youtube.com/watch?v=SUiWfhAhgQw" target="_blank">Jimmy Bogard: Vertical Slice Architecture</a>
- <a href="https://github.com/jasontaylordev/CleanArchitecture" target="_blank">Jason Taylor: Clean Architecture</a>
- <a href="https://github.com/dotnet-architecture/eShopOnContainers" target="_blank">Microsoft: eShopOnContainers</a>
- <a href="https://github.com/dotnet-architecture/eShopOnWeb" target="_blank">Microsoft: eShopOnWeb</a>
- <a href="https://github.com/kgrzybek/sample-dotnet-core-cqrs-api" target="_blank">Kamil Grzybek: Sample .NET Core REST API CQRS</a>
- <a href="https://github.com/kgrzybek/modular-monolith-with-ddd" target="_blank">Kamil Grzybek: Modular Monolith With DDD</a>

## About:

The SharedKernel Project was developed by <a href="https://www.linkedin.com/in/jordiaragonzaragoza/" target="_blank">Jordi Aragón Zaragoza</a> 
