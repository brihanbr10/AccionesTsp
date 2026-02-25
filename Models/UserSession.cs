namespace ActividadApp.Models;

public class UserSession
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string Token { get; set; } = string.Empty;
}

