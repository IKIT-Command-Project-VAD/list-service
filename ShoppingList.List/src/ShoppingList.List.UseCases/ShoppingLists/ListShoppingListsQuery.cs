namespace ShoppingList.List.UseCases.ShoppingLists;

public record ListShoppingListsQuery(Guid OwnerId) : IQuery<Result<List<ShoppingListEntity>>>;

public sealed class ListShoppingListsHandler(IReadRepository<ShoppingListEntity> repository)
    : IQueryHandler<ListShoppingListsQuery, Result<List<ShoppingListEntity>>>
{
    public async Task<Result<List<ShoppingListEntity>>> Handle(
        ListShoppingListsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListsWithDetailsSpec(request.OwnerId);
        var lists = await repository.ListAsync(spec, cancellationToken);
        return Result.Success(lists);
    }
}
