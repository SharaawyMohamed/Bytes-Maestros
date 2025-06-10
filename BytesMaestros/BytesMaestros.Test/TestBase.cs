using BytesMaestros.Application.MappingProfiles;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using BytesMaestros.Persistence.Data;
using BytesMaestros.Persistence.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
namespace BytesMaestros.Test
{
	public class TestBase : IDisposable
	{
		private readonly string _dbName;
		protected readonly DbContextOptions<BytesMaestorsDbContext> _dbContextOptions;
		protected readonly ServiceProvider _serviceProvider;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly WebApplicationBuilder _builder;

		public TestBase()
		{

			_dbContextOptions = new DbContextOptionsBuilder<BytesMaestorsDbContext>()
			.UseInMemoryDatabase(_dbName = Guid.NewGuid().ToString()).Options;

			var services = new ServiceCollection();

			var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

			var mockMediator = new Mock<IMediator>();

			_builder = WebApplication.CreateBuilder();
			_builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			AddServices(services, mockWebHostEnvironment, _builder.Configuration, mockMediator);

			_serviceProvider = services.BuildServiceProvider();
			_scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            InitializeDatabase().GetAwaiter();
		}
		private void AddServices(ServiceCollection services, Mock<IWebHostEnvironment> mockWebHostEnvironment, IConfiguration configuration, Mock<IMediator> mockMediator)
		{

			services.AddDbContext<BytesMaestorsDbContext>(options =>
			{
				options.UseInMemoryDatabase(_dbName);
			});

			services.AddLogging(builder =>
			{
				builder.AddConsole();
				builder.AddDebug();
			});

			var config = TypeAdapterConfig.GlobalSettings;
			config.Scan(Assembly.GetAssembly(new OrderMapping().GetType())!);
			services.AddSingleton(config);
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddMemoryCache();


		}

		public IUnitOfWork GetUnitOfWork()
		{
			return _serviceProvider.GetRequiredService<IUnitOfWork>();
		}
        public async Task AddRangeAsync<TEntity,TKey>(List<TEntity> entities) where TEntity: BaseEntity<TKey>
        {
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<BytesMaestorsDbContext>();
			await context.AddRangeAsync(entities);
			await context.SaveChangesAsync();
		}

		public void Dispose()
		{
			var context = _serviceProvider.GetRequiredService<BytesMaestorsDbContext>();
			context.Database.EnsureDeleted();
		}
        
		private async Task InitializeDatabase()
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<BytesMaestorsDbContext>();
				await context.Database.EnsureCreatedAsync();
			}
		}
	}
}
