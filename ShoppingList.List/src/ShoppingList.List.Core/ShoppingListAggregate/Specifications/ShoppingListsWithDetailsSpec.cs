namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListsWithDetailsSpec : Specification<ShoppingListEntity>
{
    public ShoppingListsWithDetailsSpec()
    {
        Query
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks);
    }
}

