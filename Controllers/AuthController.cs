using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using panel_Admin.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace panel_Admin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;

    private readonly IConfiguration _configuration;

	public AuthController(
        AppDbContext context,
        PasswordService passwordService,
        IConfiguration configuration)
	{
        _context = context;
        _passwordService = passwordService;
        _configuration = configuration;
	}

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.NombreUsuario == request.NombreUsuario);

        if (usuario == null)
        {
            return Unauthorized(new
{
    mensaje = "Usuario o contraseña incorrectos."
});
        }

        var contraseñaCorrecta = _passwordService.VerificarPassword(
            usuario,
            request.Password,
            usuario.PasswordHash);

        if (!contraseñaCorrecta)
        {
            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var clave = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(
           _configuration["Jwt:Key"]!));

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "panel-admin",
            audience: "panel-admin",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credenciales);

        var tokenGenerado = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            mensaje = "Login correcto.",
            usuario = usuario.NombreUsuario,
            token = tokenGenerado
        });
    }
}

public class LoginRequest
{
    public string NombreUsuario { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
