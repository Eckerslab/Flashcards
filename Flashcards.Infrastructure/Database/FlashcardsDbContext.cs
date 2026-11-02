using Flashcards.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Database;

public class FlashcardsDbContext(DbContextOptions<FlashcardsDbContext> options) : DbContext(options)
{
    public DbSet<BoxModel> Boxes => Set<BoxModel>();
    public DbSet<FlashcardModel> Cards => Set<FlashcardModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoxModel>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.HasIndex(x => x.UserId);
        });
        modelBuilder.Entity<FlashcardModel>(f =>
        {
            f.Property(x => x.Question).HasMaxLength(500).IsRequired();
            f.Property(x => x.Answer).HasMaxLength(500).IsRequired();
            f.HasIndex(x => new { x.BoxId, x.DueDate });
            f.HasOne<BoxModel>().WithMany().HasForeignKey(x => x.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}