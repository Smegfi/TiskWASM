using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;

namespace TiskWASM.Server.Data.Repositories
{
    public class CommentRepository
    {
        private DatabaseContext context;
        public CommentRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }
        public List<dtComment> Read()
        {
            return context.Comments.ToList();
        }
        public async Task<List<dtComment>> ReadAsync()
        {
            return await context.Comments.ToListAsync();
        }
        public async Task<List<dtComment>> FindBySolution(int id)
        {
            return await context.Comments.Where(x => x.SolutionId == id).ToListAsync();
        }
        public void Create(dtComment model)
        {
            this.context.Comments.Add(model);
            this.context.SaveChanges();
        }

        public async Task<dtComment> CreateAsync(dtComment model)
        {
            var result = this.context.Comments.Add(model);
            await this.context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
