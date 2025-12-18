namespace ShoppingList.List.UseCases.Categories;

public record ListCategoriesQuery() : IQuery<Result<List<Category>>>;

public sealed class ListCategoriesHandler(IReadRepository<Category> repository)
    : IQueryHandler<ListCategoriesQuery, Result<List<Category>>>
{
    public async Task<Result<List<Category>>> Handle(
        ListCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await repository.ListAsync(cancellationToken);
        return Result.Success(categories);
    }
}
