namespace ShoppingList.List.UseCases.Categories;

public record GetCategoryQuery(Guid Id) : IQuery<Result<Category>>;

public sealed class GetCategoryHandler(IReadRepository<Category> repository)
    : IQueryHandler<GetCategoryQuery, Result<Category>>
{
    public async Task<Result<Category>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        return category is null ? Result.NotFound() : Result.Success(category);
    }
}
