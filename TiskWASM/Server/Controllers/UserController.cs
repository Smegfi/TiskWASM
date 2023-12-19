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
    }
}
