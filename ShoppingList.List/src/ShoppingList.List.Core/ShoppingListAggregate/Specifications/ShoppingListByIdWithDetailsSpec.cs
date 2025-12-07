namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShoppingListByIdWithDetailsSpec
    : Specification<ShoppingListEntity>, ISingleResultSpecification
{
    public ShoppingListByIdWithDetailsSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks);
    }
}

