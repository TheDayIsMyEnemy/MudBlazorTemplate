using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MudBlazorTemplate.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(p => p.FirstName)
                .HasMaxLength(MaxLengths.StringColumn);
            builder
                .Property(p => p.LastName)
                .HasMaxLength(MaxLengths.StringColumn);
        }
    }
}
