namespace ShoppingList.List.Core.ShoppingListAggregate.Specifications;

public sealed class ShareLinkByTokenWithListSpec : Specification<ShareLink>, ISingleResultSpecification<ShareLink>
{
    public ShareLinkByTokenWithListSpec(string token)
    {
        Query
            .Where(x => x.ShareToken == token)
            .Include(x => x.List);
    }
}
