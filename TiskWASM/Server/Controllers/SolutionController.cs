using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TiskWASM.Server.Data;
using TiskWASM.Shared;
using TiskWASM.Shared.Forms;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private DatabaseContext context;
        public SolutionController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await this.context.Solutions.ToListAsync();
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
                var result = await this.context.Solutions.FindAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("without-device")]
        public async Task<IActionResult> GetSolutionWithouDevice()
        {
            try
            {
                var result = this.context.Solutions.Where(x => x.UserId != 6);
                return Ok(result);
            }   
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-user/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var user = await this.context.Solutions.FirstOrDefaultAsync(x=>x.UserId == id);

                if (user is not null)
                {
                    return Ok(true);
                }
                else 
                { 
                    return Ok(false); 
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("withoutprinter")]
        //public async Task<IActionResult> GetWithouPrinter()
        //{
        //    try
        //    {
        //        var usr = await context.Users.ToListAsync();
        //        var usersWithouPrinter = new List<dtUser>();
        //        foreach (var user in usr)
        //        {
        //            if (!await context.Solutions.AnyAsync(x => x.UserId == user.Id))
        //            {
        //                usersWithouPrinter.Add(user);
        //            }
        //        }
        //        return Ok(usersWithouPrinter);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Create(FormSolution model)
        {
            try
            {
                dtSolution solution = new dtSolution() { Id = 0, UserId = model.UserId, RequestedDevice = model.RequestedDevice, SuggestedDevice = model.SuggestedDevice, Description = model.Description };
                var result = await context.Solutions.AddAsync(solution);
                await context.SaveChangesAsync();
                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(dtSolution model)
        {
            try
            {
                var old = await this.context.Solutions.FindAsync(model.Id);
                old.Description = model.Description;
                old.RequestedDevice = model.RequestedDevice;
                old.SuggestedDevice = model.SuggestedDevice;
                old.UserId = model.UserId;
                await this.context.SaveChangesAsync();

                return Ok(old);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                this.context.Remove(await this.context.Solutions.FindAsync(id));
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
