namespace TiskWASM.Server.Data.Repositories
{
    public class CommentRepository
    {
        private DatabaseContext context;
        public CommentRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }
    }
}
