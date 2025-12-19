using FluentValidation;

namespace ShoppingList.List.Web.ShoppingLists.Create;

public class CreateShoppingListValidator : Validator<CreateShoppingListRequest>
{
    public CreateShoppingListValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be not empty");
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
