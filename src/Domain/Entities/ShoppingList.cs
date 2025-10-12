namespace ListService.Domain.Entities;

public sealed class ShoppingList
{
    public Guid ListId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = null!;
    
    public long Version { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<ListItem> Items { get; set; } = new List<ListItem>();
    public ICollection<ShareLink> ShareLinks { get; set; } = new List<ShareLink>();
}