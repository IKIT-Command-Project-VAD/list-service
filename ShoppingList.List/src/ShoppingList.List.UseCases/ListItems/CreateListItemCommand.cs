namespace ShoppingList.List.UseCases.ListItems;

public record CreateListItemCommand(
    Guid ListId,
    Guid OwnerId,
    string Name,
    decimal Quantity,
    string? Unit,
    Guid? CategoryId,
    decimal? Price,
    string? Currency,
    bool IsChecked
) : ICommand<Result<ListItem>>;

public sealed class CreateListItemHandler(
    IRepository<ShoppingListEntity> listRepo,
    IRepository<ListItem> itemRepo
) : ICommandHandler<CreateListItemCommand, Result<ListItem>>
{
    public async Task<Result<ListItem>> Handle(
        CreateListItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.ListId, request.OwnerId);
        var list = await listRepo.FirstOrDefaultAsync(spec, cancellationToken);
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

        // Persist the new item separately to avoid treating it as Modified
        await itemRepo.AddAsync(item, cancellationToken);

        // Persist list metadata updates (UpdatedAt/Version)
        await listRepo.UpdateAsync(list, cancellationToken);
        return Result.Success(item);
    }
}
