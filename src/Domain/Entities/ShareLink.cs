using ListService.Domain.Enums;

namespace ListService.Domain.Entities;

public sealed class ShareLink
{
    public Guid ShareId { get; set; }
    public Guid ListId { get; set; }
    public ShoppingList List { get; set; } = null!;

    public string ShareToken { get; set; } = null!;
    public SharePermissionType PermissionType { get; set; }
    public Guid CreatedBy { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}