using Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations
{
    internal class ScoreConfiguration : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable("scores")
                .HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id");

            builder.Property(s => s.Balance)
                .HasColumnName("balance");

            builder.Property(s => s.Percent)
                .HasColumnName("percent");

            builder.Property(s => s.DateScore)
                .HasColumnName("date_score")
                .HasColumnType("timestamp with time zone");

            builder.Property(s => s.IsCapitalization)
                .HasColumnName("is_capitalization")
                .IsRequired();

            builder.Property(s => s.IsMoney)
                .HasColumnName("is_money")
                .IsRequired();

            builder.Property(s => s.Deadline)
                .HasColumnName("deadline")
                .HasColumnType("timestamp with time zone");

            builder.Property(s => s.DateLastDividends)
                .HasColumnName("date_last_dividends")
                .HasColumnType("timestamp with time zone");


            builder.Property(s => s.ScoreType)
                .HasConversion<string>()
                .HasColumnName("score_type")
                .IsRequired();

            builder.Property(s => s.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(s => s.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            builder.HasOne<Client>()
                .WithMany(c => c.Scores)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
