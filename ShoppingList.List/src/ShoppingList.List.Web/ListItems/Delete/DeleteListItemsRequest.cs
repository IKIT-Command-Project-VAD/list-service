namespace ShoppingList.List.Web.ListItems.Delete;

public record DeleteListItemsRequest
{
    public Guid ListId { get; set; }
    public Guid[] ListItemIds { get; set; } = [];
}
