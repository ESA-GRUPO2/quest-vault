
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using questvault.Models;

namespace questvault.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
  {
    public DbSet<TwoFactorAuthenticationTokens> EmailTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<TwoFactorAuthenticationTokens>().HasKey(t => t.UserId);

      builder.Entity<Game>()
          .HasIndex(g => g.IgdbId)
          .IsUnique();

      builder.Entity<Platform>()
          .HasIndex(g => g.IgdbPlatformId)
          .IsUnique();


      builder.Entity<Company>()
          .HasIndex(g => g.IgdbCompanyId)
          .IsUnique();

      builder.Entity<Friendship>()
          .HasKey(f => new { f.User1Id, f.User2Id });

      builder.Entity<Friendship>()
          .HasOne(f => f.User1)
          .WithMany()
          .HasForeignKey(f => f.User1Id)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Friendship>()
          .HasOne(f => f.User2)
          .WithMany()
          .HasForeignKey(f => f.User2Id)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FriendshipRequest>()
          .HasKey(fr => new { fr.SenderId, fr.ReceiverId });

      builder.Entity<FriendshipRequest>()
          .HasOne(fr => fr.Sender)
          .WithMany()
          .HasForeignKey(fr => fr.SenderId)
          .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<FriendshipRequest>()
          .HasOne(fr => fr.Receiver)
          .WithMany()
          .HasForeignKey(fr => fr.ReceiverId)
          .OnDelete(DeleteBehavior.Restrict);
    }
    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<Platform> Platforms { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<GameCompany> GameCompany { get; set; } = default!;
    public DbSet<GameGenre> GameGenre { get; set; } = default!;
    public DbSet<GamePlatform> GamePlatform { get; set; } = default!;
    public DbSet<GameLog> GameLog { get; set; } = default!;
    public DbSet<GamesLibrary> GamesLibrary { get; set; } = default!;
    public DbSet<Friendship> Friendship { get; set; } = default!;
    public DbSet<FriendshipRequest> FriendshipRequest { get; set; } = default!;

  }
}
