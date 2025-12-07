namespace ShoppingList.List.UseCases.ShoppingLists;

public record ListShoppingListsQuery() : IQuery<Result<List<ShoppingListEntity>>>;

public sealed class ListShoppingListsHandler(IReadRepository<ShoppingListEntity> repository)
    : IQueryHandler<ListShoppingListsQuery, Result<List<ShoppingListEntity>>>
{
    public async Task<Result<List<ShoppingListEntity>>> Handle(
        ListShoppingListsQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListsWithDetailsSpec();
        var lists = await repository.ListAsync(spec, cancellationToken);
        return Result.Success(lists);
    }
}
