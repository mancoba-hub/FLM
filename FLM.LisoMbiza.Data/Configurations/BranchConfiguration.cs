using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLM.LisoMbiza
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey("ID").Metadata.IsPrimaryKey();
            builder.Property(x => x.Name);
            builder.Property(x => x.TelephoneNumber);
            builder.Property(x => x.OpenDate);
        }
    }
}
