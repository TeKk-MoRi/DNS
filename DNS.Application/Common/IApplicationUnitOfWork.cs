using DNS.Domain.Entities;
using DNS.Domain.Entities.Orders;

namespace DNS.Application.Common;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Save all entities in to database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IApplicationUnitOfWork : IUnitOfWork
{
    public DbSet<User> Users { get; }
    public DbSet<Order> Orders { get; }
}
