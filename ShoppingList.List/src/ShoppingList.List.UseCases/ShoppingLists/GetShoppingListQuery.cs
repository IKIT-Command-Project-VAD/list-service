namespace ShoppingList.List.UseCases.ShoppingLists;

public record GetShoppingListQuery(Guid Id, Guid OwnerId) : IQuery<Result<ShoppingListEntity>>;

public sealed class GetShoppingListHandler(IReadRepository<ShoppingListEntity> repository)
    : IQueryHandler<GetShoppingListQuery, Result<ShoppingListEntity>>
{
    public async Task<Result<ShoppingListEntity>> Handle(
        GetShoppingListQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.Id, request.OwnerId);
        var list = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        return list is null ? Result.NotFound() : Result.Success(list);
    }
}
