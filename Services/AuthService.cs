using ActividadApp.Auth;
using ActividadApp.Data;
using ActividadApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActividadApp.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    private readonly CustomAuthenticationStateProvider _authStateProvider;

    public AuthService(
        AppDbContext context, 
        JwtService jwtService,
        CustomAuthenticationStateProvider authStateProvider)
    {
        _context = context;
        _jwtService = jwtService;
        _authStateProvider = authStateProvider;
    }

    public async Task<(bool Success, string Token, string Message)> SignUp(
        string username, string email, string password, string nombre,
        int cedula, int organizacionId, int cargoId, int agenciaId)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return (false, string.Empty, "El email ya está registrado");
            }

            var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUsername != null)
            {
                return (false, string.Empty, "El nombre de usuario ya está en uso");
            }

            var user = new Usuario
            {
                Username = username,
                Email = email,
                Nombre = nombre,
                Cedula = cedula,
                OrganizacionId = organizacionId,
                CargoId = cargoId,
                AgenciaId = agenciaId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RolId = 1,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Recargar el usuario con la relación Rol
            user = await _context.Users
                .Include(u => u.Rol)
                .FirstAsync(u => u.Id == user.Id);

            var token = _jwtService.GenerateToken(user);
            await _authStateProvider.MarkUserAsAuthenticated(token);

            return (true, token, "Usuario registrado exitosamente");
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"Error al registrar usuario: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Token, string Message)> SignIn(string email, string password)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return (false, string.Empty, "Email o contraseña incorrectos");
            }

            var token = _jwtService.GenerateToken(user);
            await _authStateProvider.MarkUserAsAuthenticated(token);

            return (true, token, "Inicio de sesión exitoso");
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"Error al iniciar sesión: {ex.Message}");
        }
    }

    public async Task SignOut()
    {
        await _authStateProvider.MarkUserAsLoggedOut();
    }

    public async Task<bool> IsAuthenticated()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<UserSession?> GetCurrentUserSession()
    {
        var token = await _authStateProvider.GetTokenAsync();
        
        if (string.IsNullOrEmpty(token))
            return null;

        var userSession = _jwtService.GetUserSessionFromToken(token);
        if (userSession != null)
        {
            userSession.Token = token;
        }

        return userSession;
    }
}


