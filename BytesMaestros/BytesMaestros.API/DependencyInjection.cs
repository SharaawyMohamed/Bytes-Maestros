using System.Runtime.CompilerServices;

namespace BytesMaestros.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAPI(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
				{
					builder.AllowAnyOrigin();
					builder.AllowAnyMethod();
					builder.AllowAnyHeader();

				});
			});

			return services;
		}
	}
}
