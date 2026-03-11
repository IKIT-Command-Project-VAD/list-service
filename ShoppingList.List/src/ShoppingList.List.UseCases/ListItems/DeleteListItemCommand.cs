namespace ShoppingList.List.UseCases.ListItems;

public record DeleteListItemCommand(Guid ListId, Guid OwnerId, Guid ItemId) : ICommand<Result>;

public sealed class DeleteListItemHandler(IRepository<ListItem> itemRepo)
    : ICommandHandler<DeleteListItemCommand, Result>
{
    public async Task<Result> Handle(
        DeleteListItemCommand request,
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

        item.SoftDelete();
        await itemRepo.UpdateAsync(item, cancellationToken);
        return Result.Success();
    }
}

