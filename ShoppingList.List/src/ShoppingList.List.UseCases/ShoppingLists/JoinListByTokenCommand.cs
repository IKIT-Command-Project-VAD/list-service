using ShoppingList.List.Core.ShoppingListAggregate.Specifications;
using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.UseCases.ShoppingLists;

public record JoinListByTokenCommand(string Token, Guid UserId) : ICommand<Result>;

public sealed class JoinListByTokenHandler(
    IRepository<ShoppingList.List.Core.ShoppingListAggregate.ShoppingList> listRepository,
    IReadRepository<ShareLink> shareLinkRepository,
    IRepository<ListMember> memberRepository)
    : ICommandHandler<JoinListByTokenCommand, Result>
{
    public async Task<Result> Handle(
        JoinListByTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var shareSpec = new ShareLinkByTokenWithListSpec(request.Token);
        var shareLink = await shareLinkRepository.FirstOrDefaultAsync(shareSpec, cancellationToken);

        if (shareLink == null || !shareLink.IsActive)
        {
            return Result.NotFound("Invalid or inactive share token.");
        }

        if (shareLink.ExpiresAt.HasValue && shareLink.ExpiresAt < DateTimeOffset.UtcNow)
        {
            return Result.Forbidden("Share link has expired.");
        }

        var listSpec = new ShoppingListByIdWithDetailsSpec(shareLink.ListId, shareLink.List!.OwnerId);
        var list = await listRepository.FirstOrDefaultAsync(listSpec, cancellationToken);

        if (list == null)
        {
            return Result.NotFound("List not found.");
        }

        var member = list.AddMember(request.UserId, shareLink.PermissionType);
        
        // Explicitly add the member if it's new
        if (member.JoinedAt > DateTimeOffset.UtcNow.AddSeconds(-5))
        {
            await memberRepository.AddAsync(member, cancellationToken);
        }
        
        await listRepository.UpdateAsync(list, cancellationToken);

        return Result.Success();
    }
}
