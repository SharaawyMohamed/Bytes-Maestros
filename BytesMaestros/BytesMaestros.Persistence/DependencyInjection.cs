using BytesMaestros.Domain.Entities;
using BytesMaestros.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
		{

			services.AddDbContext<BytesMaestorsDbContext>(options =>
			{
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
			});

			services.AddIdentity<Customer, IdentityRole>()
				.AddEntityFrameworkStores<BytesMaestorsDbContext>()
				.AddDefaultTokenProviders();

			return services;
		}
	}
}
