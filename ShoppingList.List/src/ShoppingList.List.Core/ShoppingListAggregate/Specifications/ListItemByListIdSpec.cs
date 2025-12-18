namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ListItemByListIdSpec : Specification<ListItem>
{
    public ListItemByListIdSpec(Guid listId)
    {
        Query.Where(x => x.ListId == listId).Include(x => x.Category);
    }
}

