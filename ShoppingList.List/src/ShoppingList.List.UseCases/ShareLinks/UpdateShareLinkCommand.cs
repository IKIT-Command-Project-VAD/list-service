namespace ShoppingList.List.UseCases.ShareLinks;

public record UpdateShareLinkCommand(
    Guid ListId,
    Guid ShareId,
    Guid OwnerId,
    SharePermissionType PermissionType,
    DateTimeOffset? ExpiresAt,
    bool IsActive
) : ICommand<Result>;

public sealed class UpdateShareLinkHandler(IRepository<ShareLinkEntity> repository)
    : ICommandHandler<UpdateShareLinkCommand, Result>
{
    public async Task<Result> Handle(
        UpdateShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShareLinkByIdSpec(request.ListId, request.ShareId, request.OwnerId);
        var link = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (link is null)
            return Result.NotFound();

        link.Update(request.PermissionType, request.ExpiresAt, request.IsActive);
        await repository.UpdateAsync(link, cancellationToken);
        return Result.Success();
    }
}

