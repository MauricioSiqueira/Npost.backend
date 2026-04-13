using npost.Core.Auth.DAO;
using npost.Core.Auth.DTO;
using npost.Core.Auth.Model;
using npost.Core;
using System.Security.Cryptography;
using npost.Middlewares;
using npost.Service;

namespace npost.Core.Auth.Service;

public class UserService(UserDAO dao, UnitOfWork unitOfWork, TokenService tokenService)
{
    public async Task CreateAsyc(UsuarioInputDTO dto)
    {
        await ValidCreateAccAsync(dto);
        var values = ToUsuario(dto);
        values.Senha = Criptografia.sha256(dto.Senha!);
        
        await dao.CreateAsync(values);
        // await _log.AddAsync(values);
        await unitOfWork.CommitAsync();
    }

    public async Task<UserLoginOutputDTO> LoginAsync(UserLoginInputDTO dto)
    {
        var email = NormalizeEmail(dto.Email);
        var usuario = await dao.GetByEmailAsync(email);

        if (usuario is null)
        {
            throw new BusinessException("Email ou senha inválidos.");
        }

        var senhaCriptografada = Criptografia.sha256(dto.password!);
        if (usuario.Senha != senhaCriptografada)
        {
            throw new BusinessException("Email ou senha inválidos.");
        }

        var output = BuildLoginOutput(usuario);
        await PersistChangesAsync();

        return output;
    }

    public async Task<UserLoginOutputDTO?> RefreshTokenAsync(UserRefreshInputDTO dto)
    {
        var refreshToken = dto.RefreshToken?.Trim();
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var refreshTokenHash = Criptografia.sha256(refreshToken);
        var usuario = await dao.GetByRefreshTokenHashAsync(refreshTokenHash, DateTime.UtcNow);
        if (usuario is null)
        {
            return null;
        }

        var output = BuildLoginOutput(usuario);
        await PersistChangesAsync();

        return output;
    }

    public async Task<UserThemePreferenceOutputDTO> UpdateThemePreferenceAsync(UserThemePreferenceInputDTO dto)
    {
        var usuario = await dao.GetByIdAsync(tokenService.GetUsuario());
        if (usuario is null)
        {
            throw new BusinessException("Usuário não encontrado.");
        }

        usuario.DarkMode = dto.DarkMode;
        await PersistChangesAsync();

        return new UserThemePreferenceOutputDTO
        {
            DarkMode = usuario.DarkMode
        };
    }

    public Task LogoutAsync()
    {
        return LogoutInternalAsync();
    }

    private Usuario ToUsuario(UsuarioInputDTO dto)
    {
        return new Usuario
        {
            Nome =  dto.Nome,
            Sobrenome =  dto.Sobrenome,
            Email = NormalizeEmail(dto.Email),
            DarkMode = false,
            DtNascimento = dto.DataDeNascimento,
            Telefone = dto.Celular,
        };
    }

    private async Task ValidCreateAccAsync(UsuarioInputDTO dto)
    {
        if ( dto.Senha != dto.ConfirmacaoSenha)
        {
            throw new BusinessException("As duas senhas devem ser iguais.");
        }

        if (dto.DataDeNascimento >= DateTime.Now)
        {
            throw new BusinessException("Data de nascimento deve ser menor que a data atual.");
        }

        var email = NormalizeEmail(dto.Email);
        var usuario = await dao.GetByEmailAsync(email);
        if (usuario is not null)
        {
            throw new BusinessException("Já existe um usuário cadastrado com este email.");
        }
    }

    private static string NormalizeEmail(string? email)
    {
        return email!.Trim().ToLowerInvariant();
    }

    private UserLoginOutputDTO BuildLoginOutput(Usuario usuario)
    {
        var accessToken = tokenService.RefreshToken(usuario.UsuarioId!.Value);
        var refreshToken = GenerateRefreshToken();

        usuario.RefreshTokenHash = Criptografia.sha256(refreshToken);
        usuario.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(Constants.RefreshTokenExpireDays);

        return new UserLoginOutputDTO
        {
            UserName = usuario.Nome + " " + usuario.Sobrenome,
            Email = usuario.Email,
            DarkMode = usuario.DarkMode,
            Token = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task LogoutInternalAsync()
    {
        var userId = tokenService.GetUsuario();
        var usuario = await dao.GetByIdAsync(userId);
        if (usuario is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(usuario.RefreshTokenHash) && usuario.RefreshTokenExpiresAt is null)
        {
            return;
        }

        usuario.RefreshTokenHash = null;
        usuario.RefreshTokenExpiresAt = null;
        await PersistChangesAsync();
    }

    private async Task PersistChangesAsync()
    {
        await unitOfWork.SaveAsync();
        await unitOfWork.CommitAsync();
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(randomBytes);
        return token
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}
