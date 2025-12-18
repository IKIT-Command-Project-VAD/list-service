namespace ShoppingList.List.UseCases.ShareLinks;

public record GetShareLinkQuery(Guid ListId, Guid ShareId, Guid OwnerId)
    : IQuery<Result<ShareLink>>;

public sealed class GetShareLinkHandler(
    IReadRepository<ShareLink> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<GetShareLinkQuery, Result<ShareLink>>
{
    public async Task<Result<ShareLink>> Handle(
        GetShareLinkQuery request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var spec = new ShareLinkByIdSpec(request.ListId, request.ShareId, request.OwnerId);
        var link = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        return link is null ? Result.NotFound() : Result.Success(link);
    }
}
