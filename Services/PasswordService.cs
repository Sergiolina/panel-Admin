using Microsoft.AspNetCore.Identity;

namespace panel_Admin.Services;

public class PasswordService
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public string HashearPassword(Usuario usuario, string password)
    {
        return _hasher.HashPassword(usuario, password);
    }

    public bool VerificarPassword(
        Usuario usuario,
        string password,
        string passwordHash)
    {
        var resultado = _hasher.VerifyHashedPassword(
            usuario,
            passwordHash,
            password);

        return resultado == PasswordVerificationResult.Success;
    }
}
