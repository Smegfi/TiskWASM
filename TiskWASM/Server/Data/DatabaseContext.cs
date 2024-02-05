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
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("PrimaryDb"));
        }

        public DbSet<dtUser> Users { get; set; }
        public DbSet<dtComment> Comments { get; set; }
        public DbSet<dtSolution> Solutions { get; set; }
        public DbSet<dtDevice> Devices { get; set; }

        private void DataCreate()
        {

            dtUser[] users = new dtUser[]
            {
                new dtUser()
                {
                    Id = 1,
                    Name = "Jožko Ferko",
                    Department = "Odbor informatiky",
                    Email = "jozko.ferko@praha10.cz",
                    Office = "K 318"
                },
                new dtUser()
                {
                    Id = 2,
                    Name = "Franta Vomáčka",
                    Department = "Odbor informatiky",
                    Email = "franta.vomacka@praha10.cz",
                    Office = "K 319"
                },
                new dtUser()
                {
                    Id = 3,
                    Name = "Lucie Vomáčková",
                    Department = "Odbor informatiky",
                    Email = "lucie.vomackova@praha10.cz",
                    Office = "K 215"
                },
                new dtUser()
                {
                    Id = 4,
                    Name = "Tereza Formanová",
                    Department = "Odbor informatiky",
                    Email = "tereza.formanova@praha10.cz",
                    Office = "K 415"
                }
            };
            dtDevice[] devices = new dtDevice[]
            {
                new dtDevice()
                {
                    Id = 1,
                    Name = "Zařízení 1",
                    Description = "Testovací zařízení 1 s parametry xyz"
                },
                new dtDevice()
                {
                    Id = 1,
                    Name = "Zařízení 2",
                    Description = "Testovací zařízení 2 bez parametrů"
                },
                new dtDevice()
                {
                    Id = 1,
                    Name = "Zařízení 3",
                    Description = "Testovací zařízení 3 s parametry všeho dobra"
                }
            };
            this.Users.AddRange(users);
            this.Devices.AddRange(devices);
        }

    }
}
