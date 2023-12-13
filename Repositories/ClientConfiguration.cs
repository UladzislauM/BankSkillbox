using Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients")
                .HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .HasColumnName("first_name");

            builder.Property(c => c.LastName)
                .HasColumnName("last_name");

            builder.Property(c => c.History)
                .HasColumnName("history");

            builder.Property(c => c.Prestige)
                .HasColumnName("prestige");

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnName("status")
                .IsRequired();

        }
    }
}
