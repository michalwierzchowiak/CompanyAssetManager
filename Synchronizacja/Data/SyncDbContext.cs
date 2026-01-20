using Core;
using Microsoft.EntityFrameworkCore;

namespace Synchronizacja.Data
{
    public class SyncDbContext : DbContext
    {
        public SyncDbContext(DbContextOptions<SyncDbContext> options) : base(options) { }

        public DbSet<Resource> Resources { get; set; }
    }
}