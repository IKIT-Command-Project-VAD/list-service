namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListByIdWithDetailsSpec
    : Specification<ShoppingList>,
        ISingleResultSpecification<ShoppingList>
{
    public ShoppingListByIdWithDetailsSpec(Guid id, Guid userId)
    {
        Query
            .AsTracking()
            .Where(x => x.Id == id)
            .Where(x => x.OwnerId == userId || x.Members.Any(m => m.UserId == userId))
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks)
            .Include(x => x.Members);
    }
}

