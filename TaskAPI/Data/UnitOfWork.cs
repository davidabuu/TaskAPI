using TaskAPI.Core;
using TaskAPI.Core.Repositories;

namespace TaskAPI.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;
        public IProjectRepository Projects { get; private set; }

        public IProjectRepository ProjectRepository => throw new NotImplementedException();

        public UnitOfWork(ApiDbContext context, ILogger logger) { 
            _context = context;
            _logger = logger;
            Projects = new ProjectRepository(_context, logger);

        }

        public async Task CompletAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
