namespace ShoppingList.List.UseCases.ListItems;

public record ListListItemsQuery(Guid ListId) : IQuery<Result<List<ListItemEntity>>>;

public sealed class ListListItemsHandler(IReadRepository<ListItemEntity> repository)
    : IQueryHandler<ListListItemsQuery, Result<List<ListItemEntity>>>
{
    public async Task<Result<List<ListItemEntity>>> Handle(
        ListListItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ListItemByListIdSpec(request.ListId);
        var items = await repository.ListAsync(spec, cancellationToken);
        return Result.Success(items);
    }
}

