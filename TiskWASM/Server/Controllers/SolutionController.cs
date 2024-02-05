using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiskWASM.Server.Data;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private DatabaseContext context;

        public SolutionController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<List<dtSolution>> Get()
        {
            return await this.context.Solutions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<dtSolution> Get(int id)
        {
            return await this.context.Solutions.FindAsync(id);
        }

        [HttpGet("by-user/{id}")]
        public async Task<bool> GetOccurenci(int id)
        {
            var user = await this.context.Solutions.FirstOrDefaultAsync(x=>x.UserId == id);

            if (user is not null)
            {
                return true;
            }
            else { return false; }
        }

        [HttpGet("withoutprinter")]
        public async Task<IActionResult> GetWithouPrinter()
        {
            try
            {
                var usr = await context.Users.ToListAsync();
                var usersWithouPrinter = new List<dtUser>();
                foreach (var user in usr)
                {
                    if (!await context.Solutions.AnyAsync(x => x.UserId == user.Id))
                    {
                        usersWithouPrinter.Add(user);
                    }
                }
                return Ok(usersWithouPrinter);
            }
            catch (Exception ex)
            {
                return BadRequest("Nepodařilo se načíst uživatele");
            }


        }

        [HttpPost]
        public async Task<IActionResult> Create(dtSolution model)
        {
            try
            {
                var result = await context.Solutions.AddAsync(model);
                await context.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch
            {
                return BadRequest("Chyba při vytváření záznamu");
            }
        }
    }
}
