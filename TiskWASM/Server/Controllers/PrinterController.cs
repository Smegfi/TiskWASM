using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private PrintersRepository repository;
        public PrinterController(IConfiguration config)
        {
            this.repository = new PrintersRepository(config);
        }

        [HttpGet]
        public async Task<List<dtPrinters>> Get()
        {
            return await repository.ReadAsync();
        }
    }
}
