namespace ShoppingList.List.UseCases.ShoppingLists;

public record UpdateShoppingListCommand(Guid Id, string Name) : ICommand<Result>;

public sealed class UpdateShoppingListHandler(IRepository<ShoppingListEntity> repository)
    : ICommandHandler<UpdateShoppingListCommand, Result>
{
    public async Task<Result> Handle(
        UpdateShoppingListCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (list is null)
            return Result.NotFound();

        list.UpdateName(request.Name);
        await repository.UpdateAsync(list, cancellationToken);
        return Result.Success();
    }
}

