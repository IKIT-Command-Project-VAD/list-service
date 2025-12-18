namespace ShoppingList.List.UseCases.ShareLinks;

public record DeleteShareLinkCommand(Guid ListId, Guid ShareId, Guid OwnerId) : ICommand<Result>;

public sealed class DeleteShareLinkHandler(IRepository<ShareLink> repository)
    : ICommandHandler<DeleteShareLinkCommand, Result>
{
    public async Task<Result> Handle(
        DeleteShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShareLinkByIdSpec(request.ListId, request.ShareId, request.OwnerId);
        var link = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (link is null)
            return Result.NotFound();

        await repository.DeleteAsync(link, cancellationToken);
        return Result.Success();
    }
}

