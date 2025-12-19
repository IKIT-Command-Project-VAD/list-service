using FluentValidation;

namespace ShoppingList.List.Web.ListItems.Update;

public class UpdateListItemValidator : Validator<UpdateListItemRequest>
{
    public UpdateListItemValidator()
    {
        RuleFor(x => x.ListId).NotEmpty().WithMessage("ListId is required");
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("ItemId is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleFor(x => x.Name).MaximumLength(200);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be >= 0");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
        RuleFor(x => x.Currency)
            .MaximumLength(10)
            .When(x => !string.IsNullOrWhiteSpace(x.Currency));
        RuleFor(x => x.Unit).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Unit));
    }
}
