namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListsWithDetailsSpec : Specification<ShoppingListEntity>
{
    public ShoppingListsWithDetailsSpec(Guid ownerId)
    {
        Query
            .Where(x => x.OwnerId == ownerId)
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks);
    }
}

