namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ListItemByIdSpec
    : Specification<ListItem>,
        ISingleResultSpecification<ListItem>
{
    public ListItemByIdSpec(Guid listId, Guid itemId)
    {
        Query
            .AsTracking()
            .Where(x => x.ListId == listId && x.Id == itemId)
            .Include(x => x.Category)
            .Include(x => x.List);
    }
}

