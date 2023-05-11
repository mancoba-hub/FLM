using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.LisoMbiza
{
    public class BranchProductConfiguration : IEntityTypeConfiguration<BranchProduct>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<BranchProduct> builder)
        {
            builder.HasKey("Id").Metadata.IsPrimaryKey();
            builder.Property(x => x.BranchID);
            builder.Property(x => x.ProductID);
        }
    }
}
