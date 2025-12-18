namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShareLinksByListIdSpec : Specification<ShareLink>
{
    public ShareLinksByListIdSpec(Guid listId)
    {
        Query.Where(x => x.ListId == listId);
    }
}


