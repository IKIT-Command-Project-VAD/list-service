using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class ListItemConfiguration : IEntityTypeConfiguration<ListItem>
{
    public void Configure(EntityTypeBuilder<ListItem> builder)
    {
        builder.ToTable("list_items");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("item_id").HasColumnType("uuid");
        builder.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(ShoppingListDataSchemaConstants.LIST_ITEM_NAME_MAX_LENGTH)
            .IsRequired();
        builder.Property(x => x.Quantity).HasColumnName("quantity").HasColumnType("numeric(10,3)");
        builder.Property(x => x.Unit)
            .HasColumnName("unit")
            .HasMaxLength(ShoppingListDataSchemaConstants.LIST_ITEM_UNIT_MAX_LENGTH);
        builder.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("uuid");
        builder.Property(x => x.Price).HasColumnName("price").HasColumnType("numeric(10,2)");
        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(ShoppingListDataSchemaConstants.LIST_ITEM_CURRENCY_MAX_LENGTH);
        builder.Property(x => x.IsChecked).HasColumnName("is_checked");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

