namespace ListService.Domain.Entities;

public sealed class ListItem
{
    public Guid ItemId { get; set; }
    public Guid ListId { get; set; }
    public ShoppingList List { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public bool IsChecked { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}