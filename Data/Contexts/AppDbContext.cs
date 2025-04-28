using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntity>(options)
{
    public DbSet<MemberProfileEntity> MemberProfiles { get; set; }
    public DbSet<MemberAddressEntity> MemberAddresses { get; set; }
    public DbSet<ProjectMemberEntity> ProjectMembers { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<ClientAddressEntity> ClientAddresses { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MemberProfiles
        modelBuilder.Entity<MemberProfileEntity>()
            .HasOne(mp => mp.User)
            .WithOne(u => u.MemberProfile);

        modelBuilder.Entity<MemberProfileEntity>()
            .HasOne(mp => mp.MemberAddress)
            .WithOne(ma => ma.MemberProfile)
            .HasForeignKey<MemberAddressEntity>(ma => ma.UserId);

        modelBuilder.Entity<MemberProfileEntity>()
            .HasMany(mp => mp.ProjectMembers)
            .WithOne(pm => pm.MemberProfile)
            .HasForeignKey(pm => pm.UserId);

        // ProjectMembers
        modelBuilder.Entity<ProjectMemberEntity>()
            .HasKey(up => new { up.UserId, up.ProjectId });

        // Projects
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.ProjectMembers)
            .WithOne(pm => pm.Project)
            .HasForeignKey(pm => pm.ProjectId);

        // Clients
        modelBuilder.Entity<ClientEntity>()
            .HasMany(c => c.Projects)
            .WithOne(p => p.Client);

        // ClientAddresses
        modelBuilder.Entity<ClientAddressEntity>()
            .HasOne(ca => ca.Client)
            .WithOne(c => c.ClientAddress);



        base.OnModelCreating(modelBuilder);
    }
}
