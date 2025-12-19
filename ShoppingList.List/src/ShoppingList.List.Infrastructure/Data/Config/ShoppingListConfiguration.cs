namespace ShoppingList.List.Infrastructure.Data.Config;

public class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingListEntity>
{
    public void Configure(EntityTypeBuilder<ShoppingListEntity> builder)
    {
        builder.ToTable("shopping_lists");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("list_id").HasColumnType("uuid");
        builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("uuid");
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(ShoppingListDataSchemaConstants.LIST_NAME_MAX_LENGTH)
            .IsRequired();
        builder.Property(x => x.Version).HasColumnName("version");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted");

        builder.HasMany(x => x.Items).WithOne(i => i.List).HasForeignKey(i => i.ListId);
        builder.HasMany(x => x.ShareLinks).WithOne(s => s.List).HasForeignKey(s => s.ListId);
    }
}

