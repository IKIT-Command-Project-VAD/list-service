namespace ShoppingList.List.UseCases.ShoppingLists;

public record DeleteShoppingListCommand(Guid Id) : ICommand<Result>;

public sealed class DeleteShoppingListHandler(IRepository<ShoppingListEntity> repository)
    : ICommandHandler<DeleteShoppingListCommand, Result>
{
    public async Task<Result> Handle(
        DeleteShoppingListCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (list is null)
            return Result.NotFound();

        list.SoftDelete();
        await repository.UpdateAsync(list, cancellationToken);
        return Result.Success();
    }
}

