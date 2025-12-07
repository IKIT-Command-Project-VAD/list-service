namespace ShoppingList.List.UseCases.ListItems;

public record CreateListItemCommand(
    Guid ListId,
    string Name,
    decimal Quantity,
    string? Unit,
    Guid? CategoryId,
    decimal? Price,
    string? Currency,
    bool IsChecked
) : ICommand<Result<ListItemEntity>>;

public sealed class CreateListItemHandler(IRepository<ListItemEntity> itemRepo, IRepository<ShoppingListEntity> listRepo)
    : ICommandHandler<CreateListItemCommand, Result<ListItemEntity>>
{
    public async Task<Result<ListItemEntity>> Handle(
        CreateListItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null)
            return Result.NotFound();

        var item = list.AddItem(
            request.Name,
            request.Quantity,
            request.Unit,
            request.CategoryId,
            request.Price,
            request.Currency,
            request.IsChecked
        );

        await listRepo.UpdateAsync(list, cancellationToken);
        return Result.Success(item);
    }
}

