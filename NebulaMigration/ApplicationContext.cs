namespace NebulaMigration
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using NebulaMigration.Configuration;
    using NebulaMigration.Models;
    using NebulaMigration.Options;

    public class ApplicationContext : DbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">Options.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationContext(IOptions<NebulaApiOptions> options)
        {
            if (options?.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.connectionString = options.Value.ConnectionString;
            if (string.IsNullOrWhiteSpace(this.connectionString))
            {
                throw new InvalidOperationException();
            }

            this.Database.Migrate();
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CookingDish> CookingDishes { get; set; }
        public DbSet<Custom> Customs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CookingDishConfiguration());
            modelBuilder.ApplyConfiguration(new CustomConfigurartion());
            modelBuilder.ApplyConfiguration(new DishConfiguration());
            base.OnModelCreating(modelBuilder);
        }

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
    }
}