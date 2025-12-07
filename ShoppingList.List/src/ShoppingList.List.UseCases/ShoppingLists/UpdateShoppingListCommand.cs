namespace ShoppingList.List.UseCases.ShoppingLists;

public record UpdateShoppingListCommand(Guid Id, Guid OwnerId, string Name) : ICommand<Result>;

public sealed class UpdateShoppingListHandler(IRepository<ShoppingListEntity> repository)
    : ICommandHandler<UpdateShoppingListCommand, Result>
{
    public async Task<Result> Handle(
        UpdateShoppingListCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.Id, request.OwnerId);
        var list = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (list is null)
            return Result.NotFound();

        list.UpdateName(request.Name);
        await repository.UpdateAsync(list, cancellationToken);
        return Result.Success();
    }
}

