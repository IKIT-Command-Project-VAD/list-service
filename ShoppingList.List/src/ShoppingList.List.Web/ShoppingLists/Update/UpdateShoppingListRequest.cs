namespace ShoppingList.List.Web.ShoppingLists.Update;

public record UpdateShoppingListRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
