namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class Category : EntityBase<Guid>, IAggregateRoot
{
    // EF Core
    private Category() { }

    public Category(string name, string? icon = null)
    {
        Id = Guid.NewGuid();
        Name = Guard.Against.NullOrWhiteSpace(name);
        Icon = icon;
    }

    public string Name { get; private set; } = default!;
    public string? Icon { get; private set; }

    public void Update(string name, string? icon)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Icon = icon;
    }
}

