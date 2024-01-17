using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection.Metadata.Ecma335;
using TiskWASM.Server.Data;
using TiskWASM.Shared;

namespace TiskWASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private DatabaseContext context;

        public DashboardController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        [HttpGet]
        public async Task<string[]> Get()
        {
            var users = await this.context.Users.ToListAsync();
            string[] departments;

            var tst = users.Select(x => x.Department).ToArray();
            departments = tst.Distinct().ToArray();

            return departments;
        }

        [HttpGet("users")]
        public async Task<DashboardCounter> GetUsersByDep(string odbor)
        {
            var ids = await this.context.Users.Where(x => x.Department == odbor).Select(x => x.Id).ToListAsync();

            var foo = new List<int>();
            foreach (var item in ids)
            {
                if (!this.context.Solutions.Any(x => x.UserId == item))
                {
                    foo.Add(item);
                }
            }

            DashboardCounter entity = new DashboardCounter();
            entity.Ids = foo.ToArray();
            entity.Count = foo.Count;
            entity.TotalCount = ids.Count;
            return entity;
        }

        [HttpGet("list-users")]
        public async Task<IActionResult> GetById(string ids)
        {
            List<int> idList = new();
            foreach (var item in ids.Split(","))
            {
                idList.Add(int.Parse(item));
            }
            try
            {
                List<dtUser> tempUsers = new();
                foreach (var item in idList)
                {
                    var search = await this.context.Users.FindAsync(item);
                    if (search != null)
                    {
                        tempUsers.Add(search);
                    }
                }
                return Ok(tempUsers);
            }
            catch
            {
                return BadRequest("Chyba při načitání uživatelů");
            }
        }
    }
}
