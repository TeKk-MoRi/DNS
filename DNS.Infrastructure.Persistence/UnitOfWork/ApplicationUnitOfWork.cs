using DNS.Application.Common;
using DNS.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DNS.Infrastructure.Persistence.Context;

public partial class ApplicationUnitOfWork(ApplicationDbContext applicationDbContext, IMediator mediator)
    : IApplicationUnitOfWork
{
    private readonly ApplicationDbContext _context = applicationDbContext;
    private readonly IMediator _mediator = mediator;

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Gather domain events before saving
            var domainEvents = _context.ChangeTracker
                .Entries<Entity>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            await _context.SaveChangesAsync(cancellationToken);


            // Publish events
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }



            // Clear events
            foreach (var entry in _context.ChangeTracker.Entries<Entity>())
            {
                entry.Entity.ClearDomainEvents();
            }

            return Result.Success();
        }
        catch (DbUpdateConcurrencyException e)
        {
            //If you want to do something
            return Result.Failure(e.Message);
        }
        catch (DbUpdateException e)
        {
            return Result.Failure(e.Message);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}