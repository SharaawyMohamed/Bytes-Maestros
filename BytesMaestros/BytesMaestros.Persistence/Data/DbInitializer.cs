using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;
namespace BytesMaestros.Persistence.Data
{
	public class DbInitializer
	{

		public static async Task InitializeAsync(BytesMaestorsDbContext context, ILogger logger)
		{
			try
			{
				var unAppliedMigrations = await context.Database.GetPendingMigrationsAsync();

				if (unAppliedMigrations.Any())
				{
					logger.LogInformation("Applying pending migrations...");
					await context.Database.MigrateAsync();
				}

				await SeedAsync(context);

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred when initializing the database");
				throw new Exception(ex.Message);
			}
		}



		public static async Task SeedAsync(BytesMaestorsDbContext _context)
		{
			if (!_context.Types.Any())
			{
				var jsonTypes = File.ReadAllText("../BytesMaestros.Persistence/Data/DataSeeding/Types.json");
				var types = JsonSerializer.Deserialize<List<Type>>(jsonTypes);
				if (types!.Count != 0)
				{
					await _context.Types.AddRangeAsync(types);
					await _context.SaveChangesAsync();
				}
			}
			if (!_context.Products.Any())
			{
				var jsonProducts = File.ReadAllText("../BytesMaestros.Persistence/Data/DataSeeding/Products.json");
				var products = JsonSerializer.Deserialize<List<Domain.Entities.Product>>(jsonProducts);
				if (products!.Count != 0)
				{
					await _context.Products.AddRangeAsync(products);
					await _context.SaveChangesAsync();
				}
			}
		}
	}
}