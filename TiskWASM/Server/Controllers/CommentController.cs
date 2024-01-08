using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private CommentRepository repository;
        private DatabaseContext context;
        public CommentController(IConfiguration config)
        {
            this.repository = new CommentRepository(config);
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<List<dtComment>> Get()
        {
            return await repository.ReadAsync();
        }
        [HttpGet("solution/{id:int}")]
        public async Task<List<dtComment>> Get(int id)
        {
            return await repository.FindBySolution(id);
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
