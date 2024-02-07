using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;

namespace TiskWASM.Server.Data
{
    public class DatabaseContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DatabaseContext(IConfiguration config)
        {
            this._configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlite(_configuration.GetConnectionString("PrimaryDb"));
        }

        public DbSet<dtUser> Users { get; set; }
        public DbSet<dtComment> Comments { get; set; }
        public DbSet<dtSolution> Solutions { get; set; }
        public DbSet<dtDevice> Devices { get; set; }
    }
}
