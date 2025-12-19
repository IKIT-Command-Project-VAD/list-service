namespace ShoppingList.List.Web.ListItems.Update;

public record UpdateListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool IsChecked { get; init; }
}
