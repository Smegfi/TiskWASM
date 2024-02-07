using Microsoft.AspNetCore.Mvc;
using TiskWASM.Server.Data;
using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        private DatabaseContext context;
        public DeviceController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await this.context.Devices.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await this.context.Devices.FindAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Find(string searchString)
        {
            try
            {
                List<dtDevice> devices = new List<dtDevice>();
                devices.AddRange(await this.context.Devices.Where(x => x.Name.ToLowerInvariant() == searchString.ToLowerInvariant()).ToListAsync());
                devices.AddRange(await this.context.Devices.Where(x => x.Description.ToLowerInvariant() == searchString.ToLowerInvariant()).ToListAsync());
                return Ok(devices);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> Post(dtDevice model)
        {
            try
            {
                var result = await this.context.Devices.AddAsync(model);
                await this.context.SaveChangesAsync();

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(dtDevice model)
        {
            try
            {
                var old = await this.context.Devices.FirstOrDefaultAsync(x => x.Id == model.Id);
                old.Name = model.Name;
                old.Description = model.Description;
                await this.context.SaveChangesAsync();
                return Ok(old);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                this.context.Devices.Remove(await this.context.Devices.FindAsync(id));
                await this.context.SaveChangesAsync();
                return Ok("Smazáno");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
