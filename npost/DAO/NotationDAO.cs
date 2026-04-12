using Microsoft.EntityFrameworkCore;
using npost.Core;
using npost.Data;
using npost.DTOs;
using npost.Models;

namespace npost.DAO;

public class NotationDAO(DataContext db, UnitOfWork unitOfWork)
{
    public async Task CreateAsync(Notation notation)
    {
        await db.Notations.AddAsync(notation);
        await unitOfWork.SaveAsync();
    }
    
    public async Task<Notation?> GetOwnedNotationAsync(Guid notationId, int userId)
    {
        return await db.Notations.FirstOrDefaultAsync(x =>
            x.NotationId == notationId && x.UserId == userId);
    }
    
    public async Task<IReadOnlyList<NotationListItemDTO>> GetListAsync(int userId)
    {
        return await db.Notations
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Title)
            .Select(x => new NotationListItemDTO
            {
                NotationId = x.NotationId,
                Title = x.Title ?? string.Empty,
            })
            .ToListAsync();
    }
}