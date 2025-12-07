namespace ShoppingList.List.UseCases.ShareLinks;

public record DeleteShareLinkCommand(Guid ListId, Guid ShareId) : ICommand<Result>;

public sealed class DeleteShareLinkHandler(IRepository<ShareLinkEntity> repository)
    : ICommandHandler<DeleteShareLinkCommand, Result>
{
    public async Task<Result> Handle(
        DeleteShareLinkCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ShareLinkByIdSpec(request.ListId, request.ShareId);
        var link = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (link is null)
            return Result.NotFound();

        await repository.DeleteAsync(link, cancellationToken);
        return Result.Success();
    }
}

