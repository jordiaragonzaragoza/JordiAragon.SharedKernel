namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Controllers
{
    using AutoMapper;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private ICommandBus commandBus = null!;
        private IQueryBus queryBus = null!;
        private IMapper mapper = null!;

        protected ICommandBus CommandBus => this.commandBus ??= this.HttpContext.RequestServices.GetRequiredService<ICommandBus>();

        protected IQueryBus QueryBus => this.queryBus ??= this.HttpContext.RequestServices.GetRequiredService<IQueryBus>();

        protected IMapper Mapper => this.mapper ??= this.HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}