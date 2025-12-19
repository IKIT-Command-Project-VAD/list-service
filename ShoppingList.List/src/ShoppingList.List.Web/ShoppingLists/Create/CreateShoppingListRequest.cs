namespace ShoppingList.List.Web.ShoppingLists.Create;

public record CreateShoppingListRequest
{
    public string Name { get; init; } = string.Empty;
}
