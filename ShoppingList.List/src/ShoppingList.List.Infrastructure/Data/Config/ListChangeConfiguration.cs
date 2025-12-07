using ShoppingList.List.Core.ShoppingListAggregate;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class ListChangeConfiguration : IEntityTypeConfiguration<ListChange>
{
    public void Configure(EntityTypeBuilder<ListChange> builder)
    {
        builder.ToTable("list_changes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("change_id").HasColumnType("uuid");
        builder.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("uuid");
        builder.Property(x => x.ChangeType).HasColumnName("change_type").HasColumnType("change_type");
        builder.Property(x => x.FieldName).HasColumnName("field_name").HasMaxLength(100);
        builder.Property(x => x.OldValue).HasColumnName("old_value");
        builder.Property(x => x.NewValue).HasColumnName("new_value");
        builder.Property(x => x.Version).HasColumnName("version");
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");

        builder.HasOne(x => x.List).WithMany().HasForeignKey(x => x.ListId);
    }
}

