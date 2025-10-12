using ListService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListService.Infrastructure.Persistence.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.ToTable("categories");
        b.HasKey(x => x.CategoryId);
        b.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("uuid");
        b.Property(x => x.Icon).HasColumnName("icon").HasMaxLength(50);
    }
}