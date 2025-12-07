namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShareLinkByIdSpec : Specification<ShareLinkEntity>, ISingleResultSpecification
{
    public ShareLinkByIdSpec(Guid listId, Guid shareId, Guid ownerId)
    {
        Query.Where(x => x.ListId == listId && x.Id == shareId && x.List!.OwnerId == ownerId);
    }
}

