using DNS.Domain.Entities;
using DNS.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace DNS.Infrastructure.Persistence.Context;

public partial class ApplicationUnitOfWork
{
    public DbSet<User> Users => _context.Set<User>();
    public DbSet<Order> Orders => _context.Set<Order>();
}