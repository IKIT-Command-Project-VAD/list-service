using ShoppingList.List.UseCases.ShoppingLists;

namespace ShoppingList.List.Web.ShoppingLists.Join;

public class JoinListByToken(IMediator mediator)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/api/lists/join/{token}");
        Roles("user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = Route<string>("token");
        if (string.IsNullOrEmpty(token))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var userId = User.GetUserIdAsGuid();
        if (userId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new JoinListByTokenCommand(token, userId.Value), ct);

        if (result.IsSuccess)
        {
            await SendNoContentAsync(ct);
            return;
        }

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (result.Status == ResultStatus.Forbidden)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        await SendErrorsAsync(400, ct);
    }
}
