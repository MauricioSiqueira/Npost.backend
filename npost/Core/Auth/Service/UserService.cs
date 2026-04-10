using npost.Core.Auth.DAO;
using npost.Core.Auth.DTO;
using npost.Core.Auth.Model;
using npost.Middlewares;

namespace npost.Core.Auth.Service;

public class UserService(UserDAO dao, UnitOfWork unitOfWork)
{
    public async Task CreateAsyc(UsuarioInputDTO dto)
    {
        ValidCreateAcc(dto);
        var values = ToUsuario(dto);
        values.Senha = Criptografia.sha256(dto.Senha!);
        
        await dao.CreateAsync(values);
        // await _log.AddAsync(values);
        await unitOfWork.CommitAsync();
    }

    private Usuario ToUsuario(UsuarioInputDTO dto)
    {
        return new Usuario
        {
            Nome =  dto.Nome,
            Sobrenome =  dto.Sobrenome,
            Email = dto.Email,
            DarkMode = false,
            DtNascimento = dto.DataDeNascimento,
            Telefone = dto.Celular,
        };
    }

    private bool ValidCreateAcc(UsuarioInputDTO dto)
    {
        if ( dto.Senha != dto.ConfirmacaoSenha)
        {
            throw new BusinessException("As duas senhas devem ser iguais.");
        }

        if (dto.DataDeNascimento >= DateTime.Now)
        {
            throw new BusinessException("Data de nascimento deve ser menor que a data atual.");
        }
        return true;
    }
}