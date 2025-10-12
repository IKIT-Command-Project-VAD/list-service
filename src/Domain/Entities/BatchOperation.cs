using ListService.Domain.Enums;

namespace ListService.Domain.Entities;

public sealed class BatchOperation
{
    public Guid BatchId { get; set; }
    public Guid ListId { get; set; }
    public ShoppingList List { get; set; } = null!;
    
    public Guid UserId { get; set; }

    public List<Dictionary<string, object>> Operations { get; set; } = new();
    
    public BatchStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}