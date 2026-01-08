namespace ShoppingList.List.Web.ListItems.Update;

public record UpdateListItemsRequest
{
    public Guid ListId { get; set; }
    public Guid[] ListItemIds { get; set; } = [];
    public bool IsChecked { get; set; }
}
