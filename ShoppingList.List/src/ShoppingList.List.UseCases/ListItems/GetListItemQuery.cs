namespace ShoppingList.List.UseCases.ListItems;

public record GetListItemQuery(Guid ListId, Guid ItemId, Guid OwnerId) : IQuery<Result<ListItemEntity>>;

public sealed class GetListItemHandler(
    IReadRepository<ListItemEntity> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<GetListItemQuery, Result<ListItemEntity>>
{
    public async Task<Result<ListItemEntity>> Handle(
        GetListItemQuery request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var spec = new ListItemByIdSpec(request.ListId, request.ItemId);
        var item = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        return item is null ? Result.NotFound() : Result.Success(item);
    }
}

