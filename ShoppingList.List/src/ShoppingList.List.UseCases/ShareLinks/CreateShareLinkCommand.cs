namespace ShoppingList.List.UseCases.ShareLinks;

public record CreateShareLinkCommand(
    Guid ListId,
    Guid OwnerId,
    Guid CreatedBy,
    SharePermissionType PermissionType,
    DateTimeOffset? ExpiresAt
) : ICommand<Result<ShareLink>>;

public sealed class CreateShareLinkHandler(IRepository<ShoppingListEntity> listRepo)
    : ICommandHandler<CreateShareLinkCommand, Result<ShareLink>>
{
    public async Task<Result<ShareLink>> Handle(
        CreateShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var link = list.AddShareLink(request.CreatedBy, request.PermissionType, request.ExpiresAt);
        await listRepo.UpdateAsync(list, cancellationToken);
        return Result.Success(link);
    }
}
