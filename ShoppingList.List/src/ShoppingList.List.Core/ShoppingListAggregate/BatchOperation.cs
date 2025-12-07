using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Core.ShoppingListAggregate;

public sealed class BatchOperation : EntityBase<Guid>
{
    // EF Core
    private BatchOperation() { }

    public Guid ListId { get; private set; }
    public ShoppingList? List { get; private set; }

    public Guid UserId { get; private set; }
    public List<Dictionary<string, object>> Operations { get; private set; } = new();

    public BatchStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static BatchOperation Create(
        Guid listId,
        Guid userId,
        List<Dictionary<string, object>> operations
    )
    {
        Guard.Against.Default(listId);
        Guard.Against.Default(userId);

        return new BatchOperation
        {
            Id = Guid.NewGuid(),
            ListId = listId,
            UserId = userId,
            Operations = operations ?? new List<Dictionary<string, object>>(),
            Status = BatchStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public void MarkProcessing()
    {
        Status = BatchStatus.Processing;
    }

    public void MarkCompleted()
    {
        Status = BatchStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Fail(string errorMessage)
    {
        Status = BatchStatus.Failed;
        ErrorMessage = errorMessage;
        CompletedAt = DateTimeOffset.UtcNow;
    }
}

