namespace ShoppingList.List.UseCases.Categories;

public record CreateCategoryCommand(string Name, string? Icon) : ICommand<Result<Category>>;

public sealed class CreateCategoryHandler(IRepository<Category> repository)
    : ICommandHandler<CreateCategoryCommand, Result<Category>>
{
    public async Task<Result<Category>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = new Category(request.Name, request.Icon);
        await repository.AddAsync(category, cancellationToken);
        return Result.Success(category);
    }
}
