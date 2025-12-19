namespace ShoppingList.List.Web.ListItems.Delete;

public record DeleteListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}
