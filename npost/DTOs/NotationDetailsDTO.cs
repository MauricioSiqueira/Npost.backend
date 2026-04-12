namespace npost.DTOs;

public class NotationDetailsDTO
{
    public Guid NotationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
