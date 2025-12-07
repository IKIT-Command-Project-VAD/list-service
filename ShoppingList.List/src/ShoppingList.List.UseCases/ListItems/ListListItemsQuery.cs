namespace ShoppingList.List.UseCases.ListItems;

public record ListListItemsQuery(Guid ListId, Guid OwnerId) : IQuery<Result<List<ListItemEntity>>>;

public sealed class ListListItemsHandler(
    IReadRepository<ListItemEntity> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<ListListItemsQuery, Result<List<ListItemEntity>>>
{
    public async Task<Result<List<ListItemEntity>>> Handle(
        ListListItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var spec = new ListItemByListIdSpec(request.ListId);
        var items = await repository.ListAsync(spec, cancellationToken);
        return Result.Success(items);
    }
}

