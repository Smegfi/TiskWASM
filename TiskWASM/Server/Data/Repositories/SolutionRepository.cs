using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;

namespace TiskWASM.Server.Data.Repositories
{
    public class SolutionRepository
    {
        private DatabaseContext context;
        public SolutionRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }
        public List<dtSolution> Read()
        {
            return context.Solutions.ToList();
        }
        public async Task<List<dtSolution>> ReadAsync()
        {
            return await context.Solutions.ToListAsync();
        }
        public void Create(dtSolution model)
        {
            this.context.Solutions.Add(model);
            this.context.SaveChanges();
        }
        public async Task<dtSolution?> ReadAsync(int id)
        {
            return await this.context.Solutions.FirstOrDefaultAsync(x=>x.Id == id);            
        }

        public async Task CreateAsync(dtSolution model)
        {
            await context.Solutions.AddAsync(model);
            await this.context.SaveChangesAsync();
        }
    }
}
