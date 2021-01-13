namespace NebulaMigration
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using NebulaMigration.Models;
    using NebulaMigration.Options;

    public class ApplicationContext : IdentityDbContext<User>
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options,
            IOptions<NebulaApiOptions> optionsNebula) : base(options)
        {
            if (optionsNebula?.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Console.WriteLine($"Connection string: {optionsNebula.Value.ConnectionString}");
            this.connectionString = optionsNebula.Value.ConnectionString;
            if (string.IsNullOrWhiteSpace(this.connectionString))
            {
                throw new InvalidOperationException();
            }

            this.Database.Migrate();
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CookingDish> CookingDishes { get; set; }

        /// <summary>
        /// Customs.
        /// </summary>
        public virtual DbSet<Custom> Customs { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            base.OnConfiguring(optionsBuilder);
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlServer(this.connectionString);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = Guid.NewGuid().ToString();
            var adminId = Guid.NewGuid().ToString();

            builder
                .Entity<IdentityRole>()
                .HasData(new IdentityRole("Admin")
                {
                    Id = adminRoleId,
                    NormalizedName = "Admin".ToUpper()
                });

            var hasher = new PasswordHasher<User>();
            var user = new User()
            {
                Id = adminId,
                EmailConfirmed = true,
                NormalizedEmail = "ADMIN@NEBULA.COM",
                Email = "admin@nebula.com",
                UserName = "admin@nebula.com",
                NormalizedUserName = "admin@nebula.com",
                OperatorId = 1,
                SecurityStamp = string.Empty,
            };
            user.PasswordHash = hasher.HashPassword(user, "Zxcvbnm,./1");
            builder
                .Entity<User>()
                .HasData(user);

            builder
                .Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminId,
                });

            builder
                .Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();

            builder
                .Entity<Category>()
                .Property(c => c.Code)
                .IsRequired();

            builder
                .Entity<Dish>()
                .Property(d => d.Consist)
                .IsRequired();

            builder
                .Entity<Dish>()
                .Property(d => d.Unit)
                .IsRequired();

            builder
                .Entity<Dish>()
                .Property(d => d.Name)
                .IsRequired();

            builder
                .Entity<Dish>()
                .Property(d => d.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}