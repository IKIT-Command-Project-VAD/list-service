namespace ShoppingList.List.UseCases.ShareLinks;

public record ListShareLinksQuery(Guid ListId) : IQuery<Result<List<ShareLinkEntity>>>;

public sealed class ListShareLinksHandler(IReadRepository<ShareLinkEntity> repository)
    : IQueryHandler<ListShareLinksQuery, Result<List<ShareLinkEntity>>>
{
    public async Task<Result<List<ShareLinkEntity>>> Handle(
        ListShareLinksQuery request,
        CancellationToken cancellationToken
    )
    {
        var links = await repository.ListAsync(
            new Specification<ShareLinkEntity>(q => q.Where(x => x.ListId == request.ListId)),
            cancellationToken
        );
        return Result.Success(links);
    }
}

