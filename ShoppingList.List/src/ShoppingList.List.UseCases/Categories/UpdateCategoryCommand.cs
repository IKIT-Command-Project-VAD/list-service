namespace ShoppingList.List.UseCases.Categories;

public record UpdateCategoryCommand(Guid Id, string Name, string? Icon) : ICommand<Result>;

public sealed class UpdateCategoryHandler(IRepository<Category> repository)
    : ICommandHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return Result.NotFound();

        category.Update(request.Name, request.Icon);
        await repository.UpdateAsync(category, cancellationToken);
        return Result.Success();
    }
}
