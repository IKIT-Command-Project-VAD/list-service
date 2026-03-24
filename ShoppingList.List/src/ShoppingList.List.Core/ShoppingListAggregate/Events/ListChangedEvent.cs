namespace ShoppingList.List.Core.ShoppingListAggregate.Events;

public sealed class ListChangedEvent(
    Guid listId,
    string reason,
    long? version = null,
    DateTimeOffset? changedAt = null
) : DomainEventBase
{
    public Guid ListId { get; } = listId;
    public string Reason { get; } = reason;
    public long? Version { get; } = version;
    public DateTimeOffset ChangedAt { get; } = changedAt ?? DateTimeOffset.UtcNow;
}

