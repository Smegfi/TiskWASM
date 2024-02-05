using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiskWASM.Server.Data;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private DatabaseContext context;
        public CommentController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<List<dtComment>> Get()
        {
            return await this.context.Comments.ToListAsync();
        }
        [HttpGet("solution/{id:int}")]
        public async Task<List<dtComment>> Get(int id)
        {
            return await this.context.Comments.Where(x=>x.SolutionId == id).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(dtComment model)
        {
            try
            {
                var result = await context.Comments.AddAsync(model);
                await this.context.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch
            {
                return BadRequest("Chyba při vytváření komentáře");
            }            
        }
    }
}
