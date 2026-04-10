using npost.Core.Auth.Model;
using npost.Data;

namespace npost.Core.Auth.DAO;

public class UserDAO(DataContext db, UnitOfWork unitOfWork)
{
    public async Task CreateAsync(Usuario model)
    {
        await db.Usuarios.AddAsync(model);
        await unitOfWork.SaveAsync();
    }
}