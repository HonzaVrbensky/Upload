using aspnet_upload.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnet_upload.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

        PasswordHasher<User> hasher = new();

        var adminRoleId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>
        {
            Id = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        });

        builder.Entity<User>().HasData(new User
        {
            Id = adminId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@localhost",
            NormalizedEmail = "ADMIN@LOCALHOST",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(new User(), "admin"),
            SecurityStamp = string.Empty
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = adminRoleId,
            UserId = adminId
        });
    }
}
