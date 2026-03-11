namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListsWithDetailsSpec : Specification<ShoppingList>
{
    public ShoppingListsWithDetailsSpec(Guid userId)
    {
        Query
            .Where(x => x.OwnerId == userId || x.Members.Any(m => m.UserId == userId))
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks)
            .Include(x => x.Members);
    }
}

