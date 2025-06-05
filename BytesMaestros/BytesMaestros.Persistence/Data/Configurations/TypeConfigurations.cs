
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;

namespace BytesMaestros.Persistence.Data.Configurations
{
	public class TypeConfigurations : IEntityTypeConfiguration<Type>
	{
		public void Configure(EntityTypeBuilder<Type> builder)
		{
			builder.ToTable("Types");

			builder.HasKey(t => t.Id);
			builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
			builder.Property(t => t.Description).HasMaxLength(500);

			builder
				.HasMany(t => t.Products)
				.WithOne(p => p.Type)
				.HasForeignKey(p => p.TypeId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
