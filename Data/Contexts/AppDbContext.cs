using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<UserProjectEntity> UserProjects { get; set; }


    /* Since registration form accepts FullName and add-team-member form accepts FirstName + LastName,
    // override method to automatically set FullName to FirstName + LastName if FullName is empty. */
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<UserProfileEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateFullName();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.UserProfile)
            .WithOne(up => up.User);

        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.UserProjects)
            .WithOne(up => up.User)
            .HasForeignKey(up => up.UserId);


        // Projects
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.UserProjects)
            .WithOne(up => up.Project)
            .HasForeignKey(up => up.ProjectId);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId);


        // User-Projects
        modelBuilder.Entity<UserProjectEntity>()
            .HasKey(up => new { up.UserId, up.ProjectId });
    }
}
