namespace TiskWASM.Server.Data.Repositories
{
    public class SolutionRepository
    {
        private DatabaseContext context;
        public SolutionRepository(IConfiguration config)
        {
            this.context = new DatabaseContext(config);
        }
    }
}
