namespace NebulaMigration.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NebulaMigration.Models;

    public class DishConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Dishes");

            builder.HasKey(o => o.Id);

            builder
            .Property<DateTime>("createdDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CreatedDate")
            .IsRequired();

            builder
            .Property<bool>("isActive")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsActive");

            builder
            .Property<string>("name")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Name")
            .IsRequired();

            builder
            .Property<string>("consist")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Consist");

            builder
            .Property<string>("unit")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Unit");

            builder
            .Property<bool>("isAvailable")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsAvailable");

            builder
            .Property<decimal>("price")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Price");

            builder
            .Property<Category>("category")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Category")
            .IsRequired();

            builder
            .Property<int>("externalId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ExternalId")
            .IsRequired();
        }
    }
}
