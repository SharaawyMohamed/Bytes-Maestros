using BytesMaestros.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;
namespace BytesMaestros.Persistence.Data
{
	public class BytesMaestorsDbContext:IdentityDbContext<Customer>
	{
		public BytesMaestorsDbContext(DbContextOptions<BytesMaestorsDbContext> options):base(options)
		{
			
		}
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }	
		public DbSet<Type> Types { get; set; }
		public DbSet<Customer> Customers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}
	}
}
