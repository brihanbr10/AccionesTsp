using System.Security.Claims;
using ActividadApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ActividadApp.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly JwtService _jwtService;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, JwtService jwtService)
    {
        _jsRuntime = jsRuntime;
        _jwtService = jwtService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var principal = _jwtService.ValidateToken(token);
            if (principal == null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
                return new AuthenticationState(_anonymous);
            }

            return new AuthenticationState(principal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        var principal = _jwtService.ValidateToken(token);
        
        if (principal != null)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
        catch
        {
            return null;
        }
    }
}
