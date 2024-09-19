namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Services
{
    using System.Security.Claims;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService, IScopedDependency
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));

            this.UserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            this.IsAuthenticated = !string.IsNullOrEmpty(this.UserId);
        }

        public string UserId { get; set; } // Temporal. Only a init is required.

        public bool IsAuthenticated { get; init; }
    }
}