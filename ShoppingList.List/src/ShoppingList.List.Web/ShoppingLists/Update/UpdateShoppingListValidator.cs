using FluentValidation;
using ShoppingList.List.Infrastructure.Data.Config;

namespace ShoppingList.List.Web.ShoppingLists.Update;

public class UpdateShoppingListValidator : Validator<UpdateShoppingListRequest>
{
    public UpdateShoppingListValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleFor(x => x.Name).MaximumLength(ShoppingListDataSchemaConstants.LIST_NAME_MAX_LENGTH);
    }
}


