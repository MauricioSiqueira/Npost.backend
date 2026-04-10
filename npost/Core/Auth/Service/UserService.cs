using npost.Core.Auth.DAO;
using npost.Core.Auth.DTO;
using npost.Core.Auth.Model;
using npost.Middlewares;
using npost.Service;

namespace npost.Core.Auth.Service;

public class UserService(UserDAO dao, UnitOfWork unitOfWork)
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

        return new UserLoginOutputDTO
        {
            UserName = usuario.Nome + " " +  usuario.Sobrenome,
            Token = TokenService.Generation(usuario.UsuarioId!.Value, npost.Core.Constants.TokenExpire)
        };
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
}
