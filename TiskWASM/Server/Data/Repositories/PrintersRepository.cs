using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;

namespace TiskWASM.Server.Data.Repositories
{
    public class PrintersRepository
    {
        private DatabaseContext context;
        public PrintersRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }
        public List<dtPrinters> Read()
        {
            return context.Printers.ToList();
        }
        public async Task<List<dtPrinters>> ReadAsync()
        {
            return await context.Printers.ToListAsync();
        }
        public void Create(dtPrinters model)
        {
            this.context.Printers.Add(model);
            this.context.SaveChanges();
        }

        public async Task CreateAsync(dtPrinters model)
        {
            this.context.Printers.Add(model);
            await this.context.SaveChangesAsync();
        }
    }
}
