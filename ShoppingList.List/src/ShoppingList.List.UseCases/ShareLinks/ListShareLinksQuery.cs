namespace ShoppingList.List.UseCases.ShareLinks;

public record ListShareLinksQuery(Guid ListId, Guid OwnerId) : IQuery<Result<List<ShareLinkEntity>>>;

public sealed class ListShareLinksHandler(
    IReadRepository<ShareLinkEntity> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<ListShareLinksQuery, Result<List<ShareLinkEntity>>>
{
    public async Task<Result<List<ShareLinkEntity>>> Handle(
        ListShareLinksQuery request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var links = await repository.ListAsync(
            new Specification<ShareLinkEntity>(q => q.Where(x => x.ListId == request.ListId)),
            cancellationToken
        );
        return Result.Success(links);
    }
}

