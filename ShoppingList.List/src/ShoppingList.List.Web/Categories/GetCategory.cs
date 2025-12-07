using ShoppingList.List.UseCases.Categories;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class GetCategory(IMediator mediator) : Endpoint<GetCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Get("/api/categories/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(GetCategoryRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryQuery(req.Id), ct);
        if (!result.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = result.Value.ToRecord();
    }
}

public record GetCategoryRequest
{
    public Guid Id { get; init; }
}

