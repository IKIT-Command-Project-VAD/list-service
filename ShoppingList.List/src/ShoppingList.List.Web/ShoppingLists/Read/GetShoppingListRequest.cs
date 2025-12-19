namespace ShoppingList.List.Web.ShoppingLists.Read;

public record GetShoppingListRequest
{
    public Guid Id { get; init; }
}
