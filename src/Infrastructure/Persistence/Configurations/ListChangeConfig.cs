using ListService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class ListChangeConfig : IEntityTypeConfiguration<ListChange>
{
    public void Configure(EntityTypeBuilder<ListChange> b)
    {
        b.ToTable("list_changes");
        b.HasKey(x => x.ChangeId);
        b.Property(x => x.ChangeId).HasColumnName("change_id").HasColumnType("uuid");
        b.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        b.Property(x => x.ChangeType).HasColumnName("change_type").HasColumnType("change_type");
        b.Property(x => x.FieldName).HasColumnName("field_name").HasMaxLength(100);
        b.Property(x => x.OldValue).HasColumnName("old_value");
        b.Property(x => x.NewValue).HasColumnName("new_value");
        b.Property(x => x.Version).HasColumnName("version");
        b.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");

        b.HasOne(x => x.List).WithMany().HasForeignKey(x => x.ListId);
    }
}