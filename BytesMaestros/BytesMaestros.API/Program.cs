using BytesMaestros.Application;
using BytesMaestros.Persistence;
using BytesMaestros.Persistence.Data;

namespace BytesMaestros.API
{
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddPersistence(builder.Configuration)
                 .AddApplication();

			
			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

			// DbInitialzer
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<Program>>();
				var context = services.GetRequiredService<BytesMaestorsDbContext>();

				await DbInitializer.InitializeAsync(context,logger);
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
