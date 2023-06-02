namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Attributes;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR;

    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IIdentityService identityService;

        public AuthorizationBehaviour(
            ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            this.currentUserService = currentUserService;
            this.identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                // Must be authenticated user
                if (this.currentUserService.UserId == null)
                {
                    // Get Ardalis.Result.Unauthorized or Ardalis.Result<T>.Unauthorized method.
                    var resultUnauthorizedMethod = typeof(TResponse).GetMethod("Unauthorized", BindingFlags.Static | BindingFlags.Public);
                    return (TResponse)resultUnauthorizedMethod.Invoke(null, null);

                    ////throw new UnauthorizedAccessException();
                }

                // Role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        foreach (var role in roles)
                        {
                            var isInRole = await this.identityService.IsInRoleAsync(this.currentUserService.UserId, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized)
                    {
                        // Get Ardalis.Result.Forbidden or Ardalis.Result<T>.Forbidden method.
                        var resultForbiddenMethod = typeof(TResponse).GetMethod("Forbidden", BindingFlags.Static | BindingFlags.Public);
                        return (TResponse)resultForbiddenMethod.Invoke(null, null);
                    }
                }

                // Policy-based authorization
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        var authorized = await this.identityService.AuthorizeAsync(this.currentUserService.UserId, policy);

                        if (!authorized)
                        {
                            // Get Ardalis.Result.Forbidden or Ardalis.Result<T>.Forbidden method.
                            var resultForbiddenMethod = typeof(TResponse).GetMethod("Forbidden", BindingFlags.Static | BindingFlags.Public);
                            return (TResponse)resultForbiddenMethod.Invoke(null, null);
                        }
                    }
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
    }
}
