namespace ShoppingList.List.UseCases.ListItems;

public record GetListItemQuery(Guid ListId, Guid ItemId) : IQuery<Result<ListItemEntity>>;

public sealed class GetListItemHandler(IReadRepository<ListItemEntity> repository)
    : IQueryHandler<GetListItemQuery, Result<ListItemEntity>>
{
    public async Task<Result<ListItemEntity>> Handle(
        GetListItemQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ListItemByIdSpec(request.ListId, request.ItemId);
        var item = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        return item is null ? Result.NotFound() : Result.Success(item);
    }
}

