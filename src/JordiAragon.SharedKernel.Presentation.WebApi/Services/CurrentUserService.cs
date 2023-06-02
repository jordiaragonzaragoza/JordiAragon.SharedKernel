namespace JordiAragon.SharedKernel.Presentation.WebApi.Services
{
    using System.Security.Claims;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService, IScopedDependency
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            this.IsAuthenticated = this.UserId != null;
        }

        public string UserId { get; set; } // Temporal. Only a init is required.

        public bool IsAuthenticated { get; init; }
    }
}