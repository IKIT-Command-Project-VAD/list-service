namespace ShoppingList.List.UseCases.ListItems;

public record UpdateListItemCommand(
    Guid ListId,
    Guid OwnerId,
    Guid ItemId,
    string Name,
    decimal Quantity,
    string? Unit,
    Guid? CategoryId,
    decimal? Price,
    string? Currency,
    bool IsChecked
) : ICommand<Result>;

public sealed class UpdateListItemHandler(IRepository<ListItem> itemRepo)
    : ICommandHandler<UpdateListItemCommand, Result>
{
    public async Task<Result> Handle(
        UpdateListItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ListItemByIdSpec(request.ListId, request.ItemId);
        var item = await itemRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (item is null || item.List is null)
            return Result.NotFound();

        bool isOwner = item.List.OwnerId == request.OwnerId;
        bool isMemberWithWrite = item.List.Members.Any(m => m.UserId == request.OwnerId && m.PermissionType == SharePermissionType.Write);

        if (!isOwner && !isMemberWithWrite)
            return Result.Forbidden();

        item.Update(
            request.Name,
            request.Quantity,
            request.Unit,
            request.CategoryId,
            request.Price,
            request.Currency,
            request.IsChecked
        );

        await itemRepo.UpdateAsync(item, cancellationToken);
        return Result.Success();
    }
}
