namespace TaskAPI.Core
{
    public interface IUnitOfWork
    {
        IProjectRepository Projects { get; }

        Task CompletAsync();
    }
}
