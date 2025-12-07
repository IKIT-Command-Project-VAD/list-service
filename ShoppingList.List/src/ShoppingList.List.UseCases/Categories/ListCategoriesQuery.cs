namespace ShoppingList.List.UseCases.Categories;

public record ListCategoriesQuery() : IQuery<Result<List<CategoryEntity>>>;

public sealed class ListCategoriesHandler(IReadRepository<CategoryEntity> repository)
    : IQueryHandler<ListCategoriesQuery, Result<List<CategoryEntity>>>
{
    public async Task<Result<List<CategoryEntity>>> Handle(
        ListCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var categories = await repository.ListAsync(cancellationToken);
        return Result.Success(categories);
    }
}

