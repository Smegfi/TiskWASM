using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private SolutionRepository repository;
        private DatabaseContext context;

        public SolutionController(IConfiguration config)
        {
            this.repository = new SolutionRepository(config);
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<List<dtSolution>> Get()
        {
            return await repository.ReadAsync();
        }

        [HttpGet("{id}")]
        public async Task<dtSolution> Get(int id)
        {
            return await repository.ReadAsync(id);
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
