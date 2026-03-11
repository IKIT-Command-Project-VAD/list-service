using ShoppingList.List.Core.ShoppingListAggregate.Specifications;

namespace ShoppingList.List.UseCases.ShareLinks;

public record CreateShareLinkCommand(
    Guid ListId,
    Guid OwnerId,
    Guid CreatedBy,
    SharePermissionType PermissionType,
    DateTimeOffset? ExpiresAt
) : ICommand<Result<ShareLink>>;

public sealed class CreateShareLinkHandler(
    IRepository<ShoppingListEntity> listRepo,
    IRepository<ShareLink> shareLinkRepo
) : ICommandHandler<CreateShareLinkCommand, Result<ShareLink>>
{
    public async Task<Result<ShareLink>> Handle(
        CreateShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShoppingListByIdWithDetailsSpec(request.ListId, request.OwnerId);
        var list = await listRepo.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (list is null || list.OwnerId != request.OwnerId)
            return Result.NotFound();

        var link = list.AddShareLink(request.CreatedBy, request.PermissionType, request.ExpiresAt);
        
        // Explicitly add the link to avoid the Update concurrency issue
        await shareLinkRepo.AddAsync(link, cancellationToken);
        
        // Update list metadata (Version/UpdatedAt)
        await listRepo.UpdateAsync(list, cancellationToken);
        
        return Result.Success(link);
    }
}
