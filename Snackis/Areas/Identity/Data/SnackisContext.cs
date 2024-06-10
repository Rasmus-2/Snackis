using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snackis.Areas.Identity.Data;
using Snackis.Models;

namespace Snackis.Data;

public class SnackisContext : IdentityDbContext<SnackisUser>
{
    public SnackisContext(DbContextOptions<SnackisContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<Snackis.Models.Category> Category { get; set; } = default!;

public DbSet<Snackis.Models.DM> DM { get; set; } = default!;

public DbSet<Snackis.Models.ForumPost> ForumPost { get; set; } = default!;

public DbSet<Snackis.Models.Group> Group { get; set; } = default!;

public DbSet<Snackis.Models.GroupMessage> GroupMessage { get; set; } = default!;

public DbSet<Snackis.Models.MyInfoPage> MyInfoPage { get; set; } = default!;

public DbSet<Snackis.Models.Invite> Invite { get; set; } = default!;

public DbSet<Snackis.Models.FlagPost> FlagPost { get; set; } = default!;
}
