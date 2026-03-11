namespace ShoppingList.List.UseCases.ListItems;

public record GetListItemQuery(Guid ListId, Guid ItemId, Guid OwnerId) : IQuery<Result<ListItem>>;

public sealed class GetListItemHandler(
    IReadRepository<ListItem> repository,
    IReadRepository<ShoppingListEntity> listRepo
) : IQueryHandler<GetListItemQuery, Result<ListItem>>
{
    public async Task<Result<ListItem>> Handle(
        GetListItemQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.ListId, request.OwnerId);
        var list = await listRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (list is null)
            return Result.NotFound();

        var itemSpec = new ListItemByIdSpec(request.ListId, request.ItemId);
        var item = await repository.FirstOrDefaultAsync(itemSpec, cancellationToken);
        return item is null ? Result.NotFound() : Result.Success(item);
    }
}
