namespace ShoppingList.List.UseCases.Categories;

public record DeleteCategoryCommand(Guid Id) : ICommand<Result>;

public sealed class DeleteCategoryHandler(IRepository<CategoryEntity> repository)
    : ICommandHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return Result.NotFound();

        await repository.DeleteAsync(category, cancellationToken);
        return Result.Success();
    }
}

