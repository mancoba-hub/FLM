using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FLM.LisoMbiza
{
    public class FLMContext : DbContext
    {
        #region Properties

        public virtual DbSet<Branch> Branch { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<BranchProduct> BranchProduct { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FLMContext"/> class.
        /// </summary>
        /// <param name="options"></param>
        public FLMContext(DbContextOptions<FLMContext> options) : base(options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FLMContext"/> class.
        /// </summary>
        public FLMContext()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creating the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            modelBuilder.Entity<Branch>(e => e.Property(e => e.ID).ValueGeneratedOnAdd());
            modelBuilder.Entity<Product>(e => e.Property(e => e.ID).ValueGeneratedOnAdd());

            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new BranchProductConfiguration());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                .SelectMany(t => t.GetProperties())
                                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                    property.SetMaxLength(4000);
            }
        }

        #endregion
    }
}
