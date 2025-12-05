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
    public class OrderConfiguration : IEntityTypeConfiguration<DNS.Domain.Entities.Orders.Order>
    {
        public void Configure(EntityTypeBuilder<DNS.Domain.Entities.Orders.Order> builder)
        {
            // Table name
            builder.ToTable("Orders");

            // Primary key (OrderId value object)
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasConversion(
                    id => id.Value,      // to DB
                    value => new OrderId(value)) // from DB
                .ValueGeneratedNever(); // because aggregate generates its own ID

            // CustomerId value object
            builder.Property(o => o.CustomerId)
                .HasConversion(
                    id => id.Value,
                    value => new CustomerId(value));

            // Address value object mapping
            builder.OwnsOne(o => o.Address, address =>
            {
                address.Property(a => a.Country).HasColumnName("Country");
                address.Property(a => a.City).HasColumnName("City");
                address.Property(a => a.PostalCode).HasColumnName("PostalCode");
                address.Property(a => a.Street).HasColumnName("Street");
            });

            // Backing field for OrderLines
            builder.Metadata
                .FindNavigation(nameof(DNS.Domain.Entities.Orders.Order.Lines))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(o => o.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
