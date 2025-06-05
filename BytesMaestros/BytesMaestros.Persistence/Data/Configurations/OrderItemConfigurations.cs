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
	public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.ToTable("OrderItems");

			builder.HasKey(oi => oi.Id);
			builder.Property(oi => oi.Quantity).IsRequired();
			builder.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(oi => oi.TotalPrice).IsRequired();

			builder
				.HasOne(oi => oi.Product)
				.WithMany()
				.HasForeignKey(oi => oi.ProductId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
