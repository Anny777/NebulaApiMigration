namespace NebulaMigration.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NebulaMigration.Models;
    using NebulaMigration.Models.Enums;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ToTable("Categories");

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
            .Property<string>("code")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Code")
            .IsRequired();

            builder
            .Property<WorkshopType>("workshopType")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("WorkshopType")
            .IsRequired();

            builder
            .Property<int>("externalId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ExternalId")
            .IsRequired();
        }
    }
}