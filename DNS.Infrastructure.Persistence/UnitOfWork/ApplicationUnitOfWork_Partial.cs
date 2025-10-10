using DNS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DNS.Infrastructure.Persistence.Context;

public partial class ApplicationUnitOfWork
{
    public DbSet<User> Users => _context.Set<User>();
}