using ListService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class ListItemConfig : IEntityTypeConfiguration<ListItem>
{
    public void Configure(EntityTypeBuilder<ListItem> b)
    {
        b.ToTable("list_items");
        b.HasKey(x => x.ItemId);
        b.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("uuid");
        b.Property(x => x.ListId).HasColumnName("list_id").HasColumnType("uuid");
        b.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        b.Property(x => x.Quantity).HasColumnName("quantity").HasColumnType("numeric(10,3)");
        b.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(50);
        b.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("uuid");
        b.Property(x => x.Price).HasColumnName("price").HasColumnType("numeric(10,2)");
        b.Property(x => x.Currency).HasColumnName("currency").HasMaxLength(3);
        b.Property(x => x.IsChecked).HasColumnName("is_checked");
        b.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
        b.Property(x => x.IsDeleted).HasColumnName("is_deleted");

        b.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }
}