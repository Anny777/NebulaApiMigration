namespace NebulaMigration
{
    using System;
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
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IOptions<NebulaApiOptions> optionsNebula) : base(options)
        {
            if (optionsNebula?.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.connectionString = optionsNebula.Value.ConnectionString;
            if (string.IsNullOrWhiteSpace(this.connectionString))
            {
                throw new InvalidOperationException();
            }

            this.Database.EnsureCreated();
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CookingDish> CookingDishes { get; set; }
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
    }
}