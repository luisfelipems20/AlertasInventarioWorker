using Microsoft.EntityFrameworkCore;

namespace AlertasInventarioWorker.Data
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options)
            : base(options) { }

        public DbSet<Repuesto> Repuestos { get; set; }
    }
}