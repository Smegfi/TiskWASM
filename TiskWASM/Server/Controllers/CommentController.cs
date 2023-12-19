using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private CommentRepository repository;
        public CommentController(IConfiguration config)
        {
            this.repository = new CommentRepository(config);
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
        public async Task<dtComment> Post(dtComment model)
        {
            return await repository.CreateAsync(model);
        }
    }
}
