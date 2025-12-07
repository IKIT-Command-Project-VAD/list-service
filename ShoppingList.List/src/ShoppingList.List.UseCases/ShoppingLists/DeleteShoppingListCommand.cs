namespace ShoppingList.List.UseCases.ShoppingLists;

public record DeleteShoppingListCommand(Guid Id, Guid OwnerId) : ICommand<Result>;

public sealed class DeleteShoppingListHandler(IRepository<ShoppingListEntity> repository)
    : ICommandHandler<DeleteShoppingListCommand, Result>
{
    public async Task<Result> Handle(
        DeleteShoppingListCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.Id, request.OwnerId);
        var list = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (list is null)
            return Result.NotFound();

        list.SoftDelete();
        await repository.UpdateAsync(list, cancellationToken);
        return Result.Success();
    }
}

