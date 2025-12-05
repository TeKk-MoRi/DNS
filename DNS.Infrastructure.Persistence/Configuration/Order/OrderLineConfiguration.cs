using DNS.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Infrastructure.Persistence.Configuration.Order
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.ToTable("OrderLines");

            // Composite key: OrderId + LineNumber
            builder.HasKey(l => new { l.OrderId, l.LineNumber });

            builder.Property(l => l.OrderId)
                .HasConversion(
                    id => id.Value,
                    value => new OrderId(value)
                );

            builder.Property(l => l.ProductId)
                .HasConversion(
                    id => id.Value,
                    value => new ProductId(value)
                );

            // Money value object (UnitPrice)
            builder.OwnsOne(l => l.UnitPrice, money =>
            {
                money.Property(m => m.Amount).HasColumnName("UnitPrice");
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
            });
        }
    }
}
