using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using TiskWASM.Server.Data;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;
using TiskWASM.Shared.Csv;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository repository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private DatabaseContext context;
        public UserController(IConfiguration config, IWebHostEnvironment env)
        {
            this.repository = new UserRepository(config);
            this.configuration = config;
            this.env = env;
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<List<dtUser>> Get()
        {
            return await this.repository.ReadAsync();
        }

        [HttpGet("{filter}")]
        public async Task<dtUser> Filter(string filter)
        {
            return await this.repository.FindByEmail(filter);
        }

        [HttpGet("{filter:int}")]
        public async Task<dtUser> FindById(int filter)
        {
            return await this.repository.FindById(filter);
        }

        [HttpGet("office/{office}")]
        public async Task<List<dtUser>> FilterByOffice(string office)
        {
            return await context.Users.Where(x => x.Office == office).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(dtUser model)
        {
            try
            {
                var result = await context.Users.AddAsync(model);
                await context.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch
            {
                return BadRequest("Chyba při vytváření uživatele");
            }
        }

        [HttpPost("upload")]
        public async Task PostFile([FromForm] IFormFile file)
        {
            var path = Path.Combine(configuration.GetValue<string>("FileUploads"), file.FileName);

            await using FileStream fs = new(path, FileMode.Create);
            await file.CopyToAsync(fs);

            await fs.FlushAsync();
            fs.Close();

            await FromFileToDatabase(path);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        private async Task FromFileToDatabase(string filename)
        {
            List<csvKontakt> users = new List<csvKontakt>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Encoding = Encoding.UTF8,   
                BadDataFound = null,
                Delimiter = ";"
            };
            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, config))
            {
                var records = new List<csvKontakt>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new csvKontakt()
                    {
                        Name = csv.GetField<string>("Jméno"),
                        Surname = csv.GetField<string>("Příjmení"),
                        Email = csv.GetField<string>("El. adresa"),
                        Department = csv.GetField<string>("Odbor"),
                        Office = csv.GetField<string>("Kancelář")
                    };
                    users.Add(record);
                }
            }

            foreach (var item in users)
            {
                dtUser u = new dtUser()
                {
                    Name = $"{item.Name} {item.Surname}",
                    Email = item.Email,
                    Department = item.Department,
                    Office = item.Office
                };

                await this.repository.CreateAsync(u);
            }
        }
    }
}
