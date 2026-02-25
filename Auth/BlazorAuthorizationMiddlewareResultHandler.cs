using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace ActividadApp.Auth;

public class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        // Let the request through to Blazor; AuthorizeRouteView handles auth via AuthenticationStateProvider
        return next(context);
    }
}
