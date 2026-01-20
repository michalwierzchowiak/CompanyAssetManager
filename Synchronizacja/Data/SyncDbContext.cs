using Core;
using Microsoft.EntityFrameworkCore;

namespace Project.SyncService.Data
{
    public class SyncDbContext : DbContext
    {
        public SyncDbContext(DbContextOptions<SyncDbContext> options) : base(options) { }

        public DbSet<Resource> Resources { get; set; }
    }
}