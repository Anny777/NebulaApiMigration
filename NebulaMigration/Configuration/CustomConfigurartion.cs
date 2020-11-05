namespace NebulaMigration.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NebulaMigration.Models;

    public class CustomConfigurartion : IEntityTypeConfiguration<Custom>
    {
        public void Configure(EntityTypeBuilder<Custom> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Customs");

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
            .Property<ICollection<CookingDish>>("cookingDishes")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CookingDishes")
            .IsRequired();

            builder
            .Property<bool>("isOpened")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsOpened")
            .IsRequired();

            builder
            .Property<ApplicationUser>("user")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("User")
            .IsRequired();

            builder
            .Property<bool>("isExportRequested")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("IsExportRequested");

            builder
            .Property<int>("tableNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("TableNumber")
            .IsRequired();

            builder
            .Property<string>("comment")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Comment");
        }
    }
}
