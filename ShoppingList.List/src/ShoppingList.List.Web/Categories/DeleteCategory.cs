using ShoppingList.List.UseCases.Categories;

namespace ShoppingList.List.Web.Categories;

public class DeleteCategory(IMediator mediator) : Endpoint<DeleteCategoryRequest>
{
    public override void Configure()
    {
        Delete("/api/categories/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteCategoryRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(req.Id), ct);
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}

public record DeleteCategoryRequest
{
    public Guid Id { get; init; }
}

