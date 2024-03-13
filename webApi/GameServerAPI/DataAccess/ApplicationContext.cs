using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationContext : DbContext
{
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    public DbSet<Player> Player { get; set; }
    public DbSet<GameRound> GameRound { get; set; }
    public DbSet<PlayerRound> PlayerRound { get; set; }
}
