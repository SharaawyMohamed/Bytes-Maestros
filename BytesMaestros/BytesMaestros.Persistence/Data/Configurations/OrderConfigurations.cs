using BytesMaestros.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Persistence.Data.Configurations
{
	public class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders");

			builder.HasKey(o => o.Id);
			builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(50);
			builder.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(50);
			builder.Property(o => o.CustomerAddress).IsRequired().HasMaxLength(100);
			builder.Property(o => o.Status).IsRequired().HasMaxLength(20);
			builder.Property(o => o.TotalAmount).IsRequired();

			builder
				.HasMany(o => o.OrderItems)
				.WithOne(oi => oi.Order)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
