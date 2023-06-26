using Microsoft.EntityFrameworkCore;
using WorkerService.Models;

namespace WorkerService.Data
{
    public class MaterializedContext : DbContext
    {
        public MaterializedContext(DbContextOptions<MaterializedContext> options) : base(options)
        {
        }

        public DbSet<WidgetLatestState> WidgetLatestState { get; set; }

    }
}
