namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListByIdWithDetailsSpec
    : Specification<ShoppingListEntity>, ISingleResultSpecification
{
    public ShoppingListByIdWithDetailsSpec(Guid id, Guid ownerId)
    {
        Query
            .Where(x => x.Id == id)
            .Where(x => x.OwnerId == ownerId)
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks);
    }
}

