using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using TiskWASM.Server.Data;
using TiskWASM.Shared;
using TiskWASM.Shared.Csv;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private DatabaseContext context;
        public UserController(IConfiguration config, IWebHostEnvironment env)
        {
            this.configuration = config;
            this.env = env;
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await this.context.Users.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await this.context.Users.FindAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{filter}")]
        public async Task<IActionResult> Get(string filter)
        {
            try
            {
                var result = await this.context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Contains(filter.ToLower()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("search/{filter}")]
        public async Task<IActionResult> GetSearch(string filter)
        {
            try
            {
                var result = await this.context.Users.Where(x => x.Email.ToLower().Contains(filter.ToLower()) || x.Name.ToLower().Contains(filter.ToLower())).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("office/{office}")]
        public async Task<IActionResult> FilterByOffice(string office)
        {
            try
            {
                var result = await context.Users.Where(x => x.Office == office).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPut]
        public async Task<IActionResult> Update(dtUser model)
        {
            try
            {
                var old = await this.context.Users.FindAsync(model.Id);
                old.Name = model.Name;
                old.Office = model.Office;
                old.Department = model.Department;
                old.Email = model.Email;
                await this.context.SaveChangesAsync();

                return Ok(old);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                this.context.Users.Remove(await context.Users.FindAsync(id));
                await context.SaveChangesAsync();
                return Ok("Smazáno");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                await this.context.Users.AddAsync(u);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
