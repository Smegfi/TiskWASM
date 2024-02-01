using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using TiskWASM.Server.Data;
using TiskWASM.Shared;
using TiskWASM.Shared.Csv;

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

        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel()
        {
            List<csvExport> export = new();

            try
            {
                List<dtPrinters> printerTypes = await context.Printers.ToListAsync();
                foreach (var item in await context.Solutions.ToListAsync())
                {
                    dtUser user = await context.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);
                    List<dtComment> comments = await context.Comments.Where(x => x.SolutionId == item.Id).ToListAsync();

                    csvExport model = new csvExport() { };
                
                    model.UserID = item.UserId;
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.Department = user.Department;
                    model.Office = user.Office;
                    model.SolutionID = item.Id;
                    model.Description = item.Description;
                    model.RequestedPrinter = printerTypes.FirstOrDefault(x => x.Id == item.RequestedPrinter).Name;
                    model.SuggestedPrinter = printerTypes.FirstOrDefault(x => x.Id == item.SuggestedPrinter).Name;
                    export.Add(model);
                }

                string path = @"P:\Praha 10\TiskWASM\TiskWASM\Client\wwwroot\files";
                string filename = Path.GetRandomFileName().Replace(".", "") + ".csv";

                using (var writer = new StreamWriter(Path.Combine(path, filename)))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecord(export);
                }

                return Ok(filename);
            }
            catch
            {
                return BadRequest("Chyba vole");
            }
        }
    }
}
