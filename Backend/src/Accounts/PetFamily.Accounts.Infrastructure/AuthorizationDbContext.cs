using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain.Roles;
using PetFamily.Accounts.Domain.Users;

namespace PetFamily.Accounts.Infrastructure;

public class AuthorizationDbContext(IConfiguration configuration) : IdentityDbContext<User,Role,Guid>
{
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    public DbSet<ParticipantAccount> ParticipantAccounts => Set<ParticipantAccount>();
    public DbSet<VolunteerAccount> VolunteerAccounts => Set<VolunteerAccount>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("Database"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("accounts");
        
        modelBuilder.Entity<User>()
            .ToTable("users");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        modelBuilder.Entity<User>()
            .Property(u => u.SocialNetworks)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetwork>>(json, JsonSerializerOptions.Default)!);

        modelBuilder.Entity<VolunteerAccount>()
            .HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<VolunteerAccount>(u => u.UserId);
        
        modelBuilder.Entity<ParticipantAccount>()
            .HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<ParticipantAccount>(u => u.UserId);
        
        modelBuilder.Entity<AdminAccount>()
            .HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(u => u.UserId);
        
        modelBuilder.Entity<Role>()
            .ToTable("roles");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        modelBuilder.Entity<Permission>()
            .ToTable("permissions");

        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();
        
        modelBuilder.Entity<Permission>()
            .Property(p => p.Description).HasMaxLength(500);
        
        modelBuilder.Entity<RolePermission>()
            .ToTable("role_permissions");
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId } );
        
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);
        
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);
            

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AuthorizationDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => {builder.AddConsole(); });
}