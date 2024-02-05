using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using MudBlazor.Extensions;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
        private IConfiguration config;

        public DashboardController(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
            this.config = config;
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
                List<dtDevice> printerTypes = await context.Devices.ToListAsync();
                foreach (var item in await context.Solutions.ToListAsync())
                {
                    dtUser user = await context.Users.FirstOrDefaultAsync(x=>x.Id == item.UserId);
                    List<dtComment> comments = await context.Comments.Where(x => x.SolutionId == item.Id).ToListAsync();

                    var model = new csvExport()
                    {
                        UserID = item.UserId,
                        Name = user.Name,
                        Email = user.Email,
                        Department = user.Department,
                        Office = user.Office,

                        SolutionID = item.Id,
                        RequestedPrinter = printerTypes.FirstOrDefault(x=>x.Id == item.RequestedPrinter).Name,
                        SuggestedPrinter = printerTypes.FirstOrDefault(x=>x.Id == item.SuggestedPrinter).Name,
                        Description = item.Description
                    };
                    try
                    {
                        model.Comment1 = comments[0].Description;
                        model.Comment2 = comments[1].Description;
                        model.Comment3 = comments[2].Description;
                    }
                    catch { }
                    export.Add(model);
                }

                string path = config.GetValue<string>("FileUploads");
                string filename = Path.GetRandomFileName().Replace(".", "") + ".csv";

                CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                configuration.Delimiter = ";";

                using (var writer = new StreamWriter(Path.Combine(path, filename), false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, configuration))
                {
                    csv.WriteRecords(export);
                }

                var buffer = System.IO.File.ReadAllBytes(Path.Combine(path,filename));

                return File(buffer, "application/csv", "export.csv");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
