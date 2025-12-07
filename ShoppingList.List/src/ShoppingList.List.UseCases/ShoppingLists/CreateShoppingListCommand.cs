namespace ShoppingList.List.UseCases.ShoppingLists;

public record CreateShoppingListCommand(Guid OwnerId, string Name)
    : ICommand<Result<Guid>>;

public sealed class CreateShoppingListHandler(IRepository<ShoppingListEntity> repository)
    : ICommandHandler<CreateShoppingListCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateShoppingListCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = ShoppingListEntity.Create(request.OwnerId, request.Name);
        await repository.AddAsync(list, cancellationToken);
        return list.Id;
    }
}

