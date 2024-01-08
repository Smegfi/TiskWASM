using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TiskWASM.Server.Data;
using TiskWASM.Server.Data.Repositories;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        private PrintersRepository repository;
        private DatabaseContext context;
        public PrinterController(IConfiguration config)
        {
            this.repository = new PrintersRepository(config);
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await context.Printers.ToListAsync());
            }
            catch
            {
                return BadRequest("Chyba při načítání tiskáren");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(dtPrinters model)
        {
            try
            {
                var result = await this.context.Printers.AddAsync(model);
                await this.context.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest("Chyba při vytváření tiskárny");
            }
        }
    }
}
