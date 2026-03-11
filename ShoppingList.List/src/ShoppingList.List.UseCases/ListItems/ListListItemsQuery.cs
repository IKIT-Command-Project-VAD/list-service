namespace ShoppingList.List.UseCases.ListItems;

public record ListListItemsQuery(Guid ListId, Guid OwnerId) : IQuery<Result<List<ListItem>>>;

public sealed class ListListItemsHandler(
    IReadRepository<ListItem> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<ListListItemsQuery, Result<List<ListItem>>>
{
    public async Task<Result<List<ListItem>>> Handle(
        ListListItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.ListId, request.OwnerId);
        var list = await listRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (list is null)
            return Result.NotFound();

        var itemSpec = new ListItemByListIdSpec(request.ListId);
        var items = await repository.ListAsync(itemSpec, cancellationToken);
        return Result.Success(items);
    }
}
