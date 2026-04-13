using npost.Core;
using npost.DAO;
using npost.DTOs;
using npost.Middlewares;
using npost.Models;

namespace npost.Service;

public class NotationService(TokenService tokenService, UnitOfWork unitOfWork, NotationDAO dao)
{
    public async Task<IReadOnlyList<NotationListItemDTO>> GetListAsync()
    {
        return await dao.GetListAsync(tokenService.GetUsuario());
    }

    public async Task<IReadOnlyList<NotationListItemDTO>> SearchByTitleAsync(string? query)
    {
        var titleQuery = query?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(titleQuery))
        {
            return await dao.GetListAsync(tokenService.GetUsuario());
        }

        return await dao.SearchByTitleAsync(tokenService.GetUsuario(), titleQuery);
    }

    public async Task<NotationDetailsDTO> GetByIdAsync(Guid notationId)
    {
        var notation = await dao.GetOwnedNotationAsync(notationId, tokenService.GetUsuario());
        return notation is null ? throw new BusinessException("notation not found"): ToDetails(notation);
    }

    public async Task<NotationDetailsDTO> CreateAsync(CreateNotationInputDTO dto)
    {
        var notation = new Notation
        {
            NotationId = Guid.NewGuid(),
            UserId = tokenService.GetUsuario(),
            Title = dto.Title!.Trim(),
            Content = dto.Content ?? string.Empty,
        };
        
        await dao.CreateAsync(notation);
        await unitOfWork.CommitAsync();

        return ToDetails(notation);
    }

    public async Task<NotationDetailsDTO> UpdateAsync(Guid notationId, UpdateNotationInputDTO dto)
    {
        var notation = await dao.GetOwnedNotationAsync(notationId, tokenService.GetUsuario());
        
        if(notation is null)
            throw new BusinessException("notation not found");
        
        var nextTitle = dto.Title!.Trim();
        var nextContent = dto.Content ?? string.Empty;

        if (notation.Title == nextTitle && notation.Content == nextContent)
        {
            return ToDetails(notation);
        }

        notation.Title = nextTitle;
        notation.Content = nextContent;

        await unitOfWork.SaveAsync();
        await unitOfWork.CommitAsync();

        return ToDetails(notation);
    }
    
    private static NotationDetailsDTO ToDetails(Notation notation)
    {
        return new NotationDetailsDTO
        {
            NotationId = notation.NotationId,
            Title = notation.Title ?? string.Empty,
            Content = notation.Content ?? string.Empty,
        };
    }
}
