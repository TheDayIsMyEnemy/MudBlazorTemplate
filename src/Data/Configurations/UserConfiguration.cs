using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MudBlazorTemplate.Data.Entities;

namespace MudBlazorTemplate.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(p => p.FullName);
            builder
                .Property(p => p.FirstName)
                .HasMaxLength(MaximumLengths.StringColumn)
                .IsRequired();
            builder
                .Property(p => p.LastName)
                .HasMaxLength(MaximumLengths.StringColumn)
                .IsRequired();
        }
    }
}
