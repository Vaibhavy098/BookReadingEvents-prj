using BookReadingEvents.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookReadingEvents.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Invitation> Invitations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigureEvent(builder);
        ConfigureComment(builder);
        ConfigureInvitation(builder);
    }

    private static void ConfigureEvent(ModelBuilder builder)
    {
        builder.Entity<Event>()
            .HasOne(e => e.Creator)
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Event>()
            .Property(e => e.Title)
            .HasMaxLength(50);

        builder.Entity<Event>()
            .Property(e => e.Location)
            .HasMaxLength(255);

        builder.Entity<Event>()
            .Property(e => e.Description)
            .HasMaxLength(50);

        builder.Entity<Event>()
            .Property(e => e.OtherDetails)
            .HasMaxLength(500);
    }

    private static void ConfigureComment(ModelBuilder builder)
    {
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        builder.Entity<Comment>()
            .HasOne(c => c.Event)
            .WithMany(e => e.Comments)
            .HasForeignKey(c => c.EventId);
    }

    private static void ConfigureInvitation(ModelBuilder builder)
    {
        builder.Entity<Invitation>()
            .HasOne(i => i.Event)
            .WithMany(e => e.Invitations)
            .HasForeignKey(i => i.EventId);

        builder.Entity<Invitation>()
            .Property(i => i.InvitedEmail)
            .HasMaxLength(256);
    }
}