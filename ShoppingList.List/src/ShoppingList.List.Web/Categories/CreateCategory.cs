using ShoppingList.List.UseCases.Categories;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class CreateCategory(IMediator mediator) : Endpoint<CreateCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Post("/api/categories");
        Roles("user");
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var createResult = await mediator.Send(new CreateCategoryCommand(req.Name, req.Icon), ct);
        var getResult = await mediator.Send(new GetCategoryQuery(createResult.Value.Id), ct);
        Response = getResult.Value.ToRecord();
    }
}

public record CreateCategoryRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; }
}

