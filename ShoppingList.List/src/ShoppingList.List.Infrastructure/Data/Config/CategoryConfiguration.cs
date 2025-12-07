using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.Infrastructure.Data.Config;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("category_id").HasColumnType("uuid");
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.Property(x => x.Icon).HasColumnName("icon").HasMaxLength(50);
    }
}

