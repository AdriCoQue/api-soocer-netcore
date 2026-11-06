using Microsoft.EntityFrameworkCore;

namespace api_soccer.Models
{
    public class SoccerContext: DbContext
    {
        public SoccerContext(DbContextOptions<SoccerContext> options) : base(options) { }
        public DbSet<League> leagues { get; set; }
        public DbSet<Team> teams { get; set; }
        public DbSet<TeamInLeague> registers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<League>()
                .Property(p => p.idleague)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<League>()
                .Property(t => t.enabled)
                .HasDefaultValue(true)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Team>()
                .Property(p => p.idteam)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Team>()
                .Property(t => t.enabled)
                .HasDefaultValue(true)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TeamInLeague>()
                .Property(t => t.enabled)
                .HasDefaultValue(true)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TeamInLeague>()
                .HasOne(p => p.league)
                .WithMany(f => f.teamsInLeague)
                .HasForeignKey(f => f.idleague);

            modelBuilder.Entity<TeamInLeague>()
                .HasOne(p => p.team)
                .WithMany(f => f.teamsInLeague)
                .HasForeignKey(f => f.idteam);

        }
    }
}
