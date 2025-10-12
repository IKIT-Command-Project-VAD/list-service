using ListService.Domain.Enums;

namespace ListService.Domain.Entities;

public class ListChange
{
    public Guid ChangeId { get; set; }
    public Guid ListId { get; set; }
    public ShoppingList List { get; set; } = null!;
    
    public Guid? ItemId { get; set; }
    public ChangeType ChangeType { get; set; }
    public string? FieldName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    
    public long Version  { get; set; }
    public Guid? UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}