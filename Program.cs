using ActividadApp.Services;
using ActividadApp.Services.Email;
using ActividadApp.Components;
using ActividadApp.Data;
using ActividadApp.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using dotenv.net;

// Cargar .env si existe (local y Docker)
DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: true));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

// JWT Configuration
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
    ?? builder.Configuration["Jwt:Key"]!;
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
    ?? builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();

// Services - Email con override de .env
var emailSection = builder.Configuration.GetSection("EmailSettings");
builder.Services.Configure<EmailSettings>(settings =>
{
    settings.SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? emailSection["SmtpServer"]!;
    settings.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? emailSection["SmtpPort"]!);
    settings.SenderName = emailSection["SenderName"]!;
    settings.SenderEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? emailSection["SenderEmail"]!;
    settings.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? emailSection["Password"]!;
    settings.UseSsl = bool.Parse(emailSection["UseSsl"] ?? "true");
});
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AccionService>();
builder.Services.AddScoped<ReporteService>();
builder.Services.AddHostedService<NotificacionBackgroundService>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.SeedAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


