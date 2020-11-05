namespace NebulaMigration.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NebulaMigration.Models;
    using NebulaMigration.Models.Enums;

    public class CookingDishConfiguration : IEntityTypeConfiguration<CookingDish>
    {
        public void Configure(EntityTypeBuilder<CookingDish> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("CookingDishes");

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
            .Property<Dish>("dish")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Dish")
            .IsRequired();

            builder
            .Property<DishState>("dishState")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("DishState")
            .IsRequired();

            builder
            .Property<Custom>("custom")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Custom")
            .IsRequired();

            builder
            .Property<string>("comment")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Comment");
        }
    }
}
