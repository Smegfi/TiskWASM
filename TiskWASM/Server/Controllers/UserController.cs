using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepository repository;
        public UserController(IConfiguration config)
        {
            this.repository = new UserRepository(config);
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

        [HttpPost]
        public async Task Create(dtUser model)
        {
            await repository.CreateAsync(model);            
        }
    }
}
