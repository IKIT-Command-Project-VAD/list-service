namespace ShoppingList.List.UseCases.Categories;

public record GetCategoryQuery(Guid Id) : IQuery<Result<CategoryEntity>>;

public sealed class GetCategoryHandler(IReadRepository<CategoryEntity> repository)
    : IQueryHandler<GetCategoryQuery, Result<CategoryEntity>>
{
    public async Task<Result<CategoryEntity>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        return category is null ? Result.NotFound() : Result.Success(category);
    }
}

