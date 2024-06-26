﻿namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Controllers
{
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private ISender internalBus = null!;
        private IMapper mapper = null!;

        protected ISender InternalBus => this.internalBus ??= this.HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IMapper Mapper => this.mapper ??= this.HttpContext.RequestServices.GetRequiredService<IMapper>();
    }
}