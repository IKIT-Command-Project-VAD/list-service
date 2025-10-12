using System.Text.Json;
using ListService.Domain.Entities;
using ListService.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class BatchOperationConfig : IEntityTypeConfiguration<BatchOperation>
{
    public void Configure(EntityTypeBuilder<BatchOperation> b)
    {
        b.ToTable("batch_operations");
        b.HasKey(x => x.BatchId);
        b.Property(x => x.BatchId).HasColumnName("batch_id").HasColumnType("uuid");
        b.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        b.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        b.Property(x => x.Status).HasColumnName("status").HasColumnType("batch_status");
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        b.Property(x => x.CompletedAt).HasColumnName("completed_at").HasColumnType("timestamptz");
        b.Property(x => x.ErrorMessage).HasColumnName("error_message");

        b.Property(x => x.Operations)
            .HasColumnName("operations")
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrWhiteSpace(v) ? new List<Dictionary<string, object>>() :
                    JsonSerializer.Deserialize<List<Dictionary<string, object>>>(v, (JsonSerializerOptions?)null)!)
            .Metadata.SetValueComparer(JsonConverters.OpsComparer);

        b.HasOne(x => x.List).WithMany().HasForeignKey(x => x.ListId);
    }
}