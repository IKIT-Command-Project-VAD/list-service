namespace ShoppingList.List.UseCases.Categories;

public record CreateCategoryCommand(string Name, string? Icon) : ICommand<Result<CategoryEntity>>;

public sealed class CreateCategoryHandler(IRepository<CategoryEntity> repository)
    : ICommandHandler<CreateCategoryCommand, Result<CategoryEntity>>
{
    public async Task<Result<CategoryEntity>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = new CategoryEntity(request.Name, request.Icon);
        await repository.AddAsync(category, cancellationToken);
        return Result.Success(category);
    }
}

