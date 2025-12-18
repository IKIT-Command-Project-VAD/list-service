namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class ListItem : EntityBase<Guid>, IAggregateRoot
{
    public Guid ListId { get; private set; }
    public ShoppingList? List { get; private set; }

    public string Name { get; private set; } = default!;
    public decimal Quantity { get; private set; }
    public string? Unit { get; private set; }

    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public decimal? Price { get; private set; }
    public string? Currency { get; private set; }
    public bool IsChecked { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    // EF Core
    private ListItem() { }

    internal static ListItem Create(
        Guid listId,
        string name,
        decimal quantity,
        string? unit,
        Guid? categoryId,
        decimal? price,
        string? currency,
        bool isChecked,
        Guid itemId
    )
    {
        Guard.Against.Default(listId);
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.Negative(quantity);

        return new ListItem
        {
            Id = itemId,
            ListId = listId,
            Name = name,
            Quantity = quantity,
            Unit = unit,
            CategoryId = categoryId,
            Price = price,
            Currency = currency,
            IsChecked = isChecked,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
        };
    }

    public void Update(
        string name,
        decimal quantity,
        string? unit,
        Guid? categoryId,
        decimal? price,
        string? currency,
        bool isChecked
    )
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.Negative(quantity);

        Quantity = quantity;
        Unit = unit;
        CategoryId = categoryId;
        Price = price;
        Currency = currency;
        IsChecked = isChecked;
        Touch();
    }

    public void ToggleChecked(bool? isChecked = null)
    {
        IsChecked = isChecked ?? !IsChecked;
        Touch();
    }

    public void SoftDelete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        Touch();
    }

    private void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
