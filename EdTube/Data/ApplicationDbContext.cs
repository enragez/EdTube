using EdTube.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Data;

public sealed class ApplicationDbContext : IdentityDbContext
{
    public DbSet<VideoCategory> Categories { get; set; } = null!;
    
    public DbSet<Video> Videos { get; set; } = null!;

    public DbSet<BecomeAuthorRequest> BecomeAuthorRequests { get; set; } = null!;
    
    public DbSet<VideoChannel> Channels { get; set; } = null!;
    
    public DbSet<TestQuestion> TestQuestions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
