﻿using BytesMaestros.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Persistence.Data.Configurations
{
	public class ProductConfigurations : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("Products");

			builder.HasKey(p => p.Id);
			builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
			builder.Property(p => p.Description).HasMaxLength(500);
			builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(p => p.Stock).IsRequired();

			builder.HasOne(p => p.Type)
				.WithMany(t => t.Products)
				.HasForeignKey(p => p.TypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(p=>p.OrderItems)
				.WithOne(p=>p.Product)
				.HasForeignKey(p=>p.ProductId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
