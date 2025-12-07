namespace ShoppingList.List.UseCases.ShareLinks;

public record GetShareLinkQuery(Guid ListId, Guid ShareId, Guid OwnerId) : IQuery<Result<ShareLinkEntity>>;

public sealed class GetShareLinkHandler(
    IReadRepository<ShareLinkEntity> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<GetShareLinkQuery, Result<ShareLinkEntity>>
{
    public async Task<Result<ShareLinkEntity>> Handle(
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

