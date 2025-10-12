namespace ListService.Domain.Entities;

public sealed class Category
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Icon  { get; set; }
}