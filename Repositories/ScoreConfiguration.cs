using Bank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories
{
    internal class ScoreConfiguration : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable("scores")
                .HasKey(s => s.Id);

            builder.Property(s => s.Balance)
                .HasColumnName("balance");

            builder.Property(s => s.Percent)
                .HasColumnName("percent");

            builder.Property(s => s.DateScore)
                .HasColumnName("date_score");

            builder.Property(s => s.IsСapitalization)
                .HasColumnName("is_capitalization")
                .IsRequired();

            builder.Property(s => s.IsMoney)
                .HasColumnName("is_money")
                .IsRequired();

            builder.Property(s => s.Deadline)
                .HasColumnName("deadline");

            builder.Property(s => s.DateLastDividends)
                .HasColumnName("date_last_dividends");


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

            builder.HasOne(s => s.Client)
                .WithMany(c => c.Scores)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
