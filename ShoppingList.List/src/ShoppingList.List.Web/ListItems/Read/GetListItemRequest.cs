namespace ShoppingList.List.Web.ListItems.Read;

public record GetListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}
