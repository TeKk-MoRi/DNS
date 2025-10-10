using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DNS.Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
}