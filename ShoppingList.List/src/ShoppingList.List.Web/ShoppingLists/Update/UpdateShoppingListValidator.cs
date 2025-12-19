using FluentValidation;

namespace ShoppingList.List.Web.ShoppingLists.Update;

public class UpdateShoppingListValidator : Validator<UpdateShoppingListRequest>
{
    public UpdateShoppingListValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleFor(x => x.Name).MaximumLength(200);
    }
}


