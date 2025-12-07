using ShoppingList.List.UseCases.Categories;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class UpdateCategory(IMediator mediator)
    : Endpoint<UpdateCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Put("/api/categories/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateCategoryRequest req, CancellationToken ct)
    {
        var updateResult = await mediator.Send(
            new UpdateCategoryCommand(req.Id, req.Name, req.Icon),
            ct
        );
        if (updateResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        if (updateResult.Status == ResultStatus.Invalid)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var getResult = await mediator.Send(new GetCategoryQuery(req.Id), ct);
        Response = getResult.Value.ToRecord();
    }
}

public record UpdateCategoryRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; }
}

