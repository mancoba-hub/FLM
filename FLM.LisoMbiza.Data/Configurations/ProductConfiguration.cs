using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.LisoMbiza
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey("ID").Metadata.IsPrimaryKey();
            builder.Property(x => x.Name);
            builder.Property(x => x.WeightedItem);
            builder.Property(x => x.SuggestedSellingPrice);
        }
    }
}
