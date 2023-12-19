using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private SolutionRepository repository;
        public SolutionController(IConfiguration config)
        {
            this.repository = new SolutionRepository(config);
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
        public async Task Create(dtSolution model)
        {
            await repository.CreateAsync(model);
        }
    }
}
