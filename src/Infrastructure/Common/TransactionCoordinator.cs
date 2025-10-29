using Core.Interfaces.Services;
using Infrastructure.Data;

namespace Infrastructure.Common
{
    public class TransactionCoordinator : ITransactionCoordinator
    {
        private readonly ApplicationDbContext _db;

        public TransactionCoordinator(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync(Func<CancellationToken, Task> work, CancellationToken ct = default)
        {
            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                await work(ct);
                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> work, CancellationToken ct = default)
        {
            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                var result = await work(ct);
                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return result;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}


