using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data.Repositories;

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
    }
}
