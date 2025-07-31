using Microsoft.EntityFrameworkCore;

namespace ScoreHub_TG;

public class TgBotDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=scorehubTG;Username=postgres;Password=12345");
        }
    }
}