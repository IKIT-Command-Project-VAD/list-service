namespace ShoppingList.List.UseCases.ShareLinks;

public record CreateShareLinkCommand(
    Guid ListId,
    Guid CreatedBy,
    SharePermissionType PermissionType,
    DateTimeOffset? ExpiresAt
) : ICommand<Result<ShareLinkEntity>>;

public sealed class CreateShareLinkHandler(IRepository<ShareLinkEntity> shareRepo, IRepository<ShoppingListEntity> listRepo)
    : ICommandHandler<CreateShareLinkCommand, Result<ShareLinkEntity>>
{
    public async Task<Result<ShareLinkEntity>> Handle(
        CreateShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var list = await listRepo.GetByIdAsync(request.ListId, cancellationToken);
        if (list is null)
            return Result.NotFound();

        var link = list.AddShareLink(request.CreatedBy, request.PermissionType, request.ExpiresAt);
        await listRepo.UpdateAsync(list, cancellationToken);
        return Result.Success(link);
    }
}

