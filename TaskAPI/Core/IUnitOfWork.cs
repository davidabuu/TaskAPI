namespace TaskAPI.Core
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }

        Task CompletAsync();
    }
}
