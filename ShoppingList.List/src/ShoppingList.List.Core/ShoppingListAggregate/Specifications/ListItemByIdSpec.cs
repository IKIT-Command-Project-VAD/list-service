namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ListItemByIdSpec : Specification<ListItemEntity>, ISingleResultSpecification
{
    public ListItemByIdSpec(Guid listId, Guid itemId)
    {
        Query.Where(x => x.ListId == listId && x.Id == itemId).Include(x => x.Category);
    }
}

