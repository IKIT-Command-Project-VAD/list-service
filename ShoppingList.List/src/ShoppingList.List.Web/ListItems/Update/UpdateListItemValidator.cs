using FluentValidation;
using ShoppingList.List.Infrastructure.Data.Config;

namespace ShoppingList.List.Web.ListItems.Update;

public class UpdateListItemValidator : Validator<UpdateListItemRequest>
{
    public UpdateListItemValidator()
    {
        RuleFor(x => x.ListId).NotEmpty().WithMessage("ListId is required");
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleFor(x => x.Name).MaximumLength(ShoppingListDataSchemaConstants.LIST_ITEM_NAME_MAX_LENGTH);
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(ShoppingListDataSchemaConstants.LIST_ITEM_MIN_QUANTITY)
            .WithMessage("Quantity must be >= 0");
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(ShoppingListDataSchemaConstants.LIST_ITEM_MIN_PRICE)
            .When(x => x.Price.HasValue);
        RuleFor(x => x.Currency)
            .MaximumLength(ShoppingListDataSchemaConstants.LIST_ITEM_CURRENCY_MAX_LENGTH)
            .When(x => !string.IsNullOrWhiteSpace(x.Currency));
        RuleFor(x => x.Unit)
            .MaximumLength(ShoppingListDataSchemaConstants.LIST_ITEM_UNIT_MAX_LENGTH)
            .When(x => !string.IsNullOrWhiteSpace(x.Unit));
    }
}
