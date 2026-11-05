using ImageProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessingService.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    private DbSet<AppUser> AppUsers { get; set; }
}