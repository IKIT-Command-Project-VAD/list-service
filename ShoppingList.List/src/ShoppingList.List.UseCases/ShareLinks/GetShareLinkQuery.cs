namespace ShoppingList.List.UseCases.ShareLinks;

public record GetShareLinkQuery(Guid ListId, Guid ShareId) : IQuery<Result<ShareLinkEntity>>;

public sealed class GetShareLinkHandler(IReadRepository<ShareLinkEntity> repository)
    : IQueryHandler<GetShareLinkQuery, Result<ShareLinkEntity>>
{
    public async Task<Result<ShareLinkEntity>> Handle(
        GetShareLinkQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShareLinkByIdSpec(request.ListId, request.ShareId);
        var link = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        return link is null ? Result.NotFound() : Result.Success(link);
    }
}

