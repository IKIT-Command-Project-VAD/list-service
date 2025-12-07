using ShoppingList.List.UseCases.Categories;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class ListCategories(IMediator mediator)
    : EndpointWithoutRequest<List<CategoryRecord>>
{
    public override void Configure()
    {
        Get("/api/categories");
        Roles("user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new ListCategoriesQuery(), ct);
        Response = result.Value.Select(c => c.ToRecord()).ToList();
    }
}

