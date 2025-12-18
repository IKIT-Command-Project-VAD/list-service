namespace ShoppingList.List.UseCases.ShareLinks;

public record ListShareLinksQuery(Guid ListId, Guid OwnerId) : IQuery<Result<List<ShareLink>>>;

public sealed class ListShareLinksHandler(
    IReadRepository<ShareLink> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<ListShareLinksQuery, Result<List<ShareLink>>>
{
    public async Task<Result<List<ShareLink>>> Handle(
        ListShareLinksQuery request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var links = await repository.ListAsync(
            new ShareLinksByListIdSpec(request.ListId),
            cancellationToken
        );
        return Result.Success(links);
    }
}
