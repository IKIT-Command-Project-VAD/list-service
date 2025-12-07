using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class ListChange : EntityBase<Guid>
{
    // EF Core
    private ListChange() { }

    public Guid ListId { get; private set; }
    public ShoppingList? List { get; private set; }

    public Guid? ItemId { get; private set; }
    public ChangeType ChangeType { get; private set; }
    public string? FieldName { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }

    public long Version { get; private set; }
    public Guid? UserId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static ListChange Record(
        Guid listId,
        ChangeType changeType,
        long version,
        Guid? userId,
        string? fieldName,
        string? oldValue,
        string? newValue,
        Guid? itemId = null
    )
    {
        Guard.Against.Default(listId);

        return new ListChange
        {
            Id = Guid.NewGuid(),
            ListId = listId,
            ItemId = itemId,
            ChangeType = changeType,
            FieldName = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            Version = version,
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }
}

