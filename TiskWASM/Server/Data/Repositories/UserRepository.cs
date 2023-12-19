using Microsoft.EntityFrameworkCore;
using TiskWASM.Shared;

namespace TiskWASM.Server.Data.Repositories
{
    public class UserRepository
    {
        private DatabaseContext context;
        public UserRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }

        public List<dtUser> Read()
        {
            return context.Users.ToList();
        }
        public async Task<List<dtUser>> ReadAsync()
        {
            return await context.Users.ToListAsync();
        }

        public void Create(dtUser user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public async Task CreateAsync(dtUser user)
        {
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();    
        }

        public async Task<dtUser?> FindByEmail(string email)
        {
            return await this.context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<dtUser?> FindById(int id)
        {
            return await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
