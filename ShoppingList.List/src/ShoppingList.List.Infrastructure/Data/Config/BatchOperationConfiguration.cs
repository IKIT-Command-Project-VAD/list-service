using System.Text.Json;
using ShoppingList.List.Core.ShoppingListAggregate;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class BatchOperationConfiguration : IEntityTypeConfiguration<BatchOperation>
{
    public void Configure(EntityTypeBuilder<BatchOperation> builder)
    {
        builder.ToTable("batch_operations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("batch_id").HasColumnType("uuid");
        builder.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(x => x.Status).HasColumnName("status").HasColumnType("batch_status");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        builder.Property(x => x.CompletedAt).HasColumnName("completed_at").HasColumnType("timestamptz");
        builder.Property(x => x.ErrorMessage).HasColumnName("error_message");

        builder
            .Property(x => x.Operations)
            .HasColumnName("operations")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v =>
                    string.IsNullOrWhiteSpace(v)
                        ? new List<Dictionary<string, object>>()
                        : JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                            v,
                            (JsonSerializerOptions?)null
                        )!
            )
            .Metadata.SetValueComparer(JsonConverters.OpsComparer);

        builder.HasOne(x => x.List).WithMany().HasForeignKey(x => x.ListId);
    }
}

