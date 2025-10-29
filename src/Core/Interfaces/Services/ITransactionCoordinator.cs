namespace Core.Interfaces.Services
{
    public interface ITransactionCoordinator
    {
        Task ExecuteAsync(Func<CancellationToken, Task> work, CancellationToken ct = default);
        Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> work, CancellationToken ct = default);
    }
}


