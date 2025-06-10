using BytesMaestros.Application.Features.Orders.Commands.CreateOrder;
using BytesMaestros.Domain.Repositories;
using BytesMaestros.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.AddMapster();

			#region Mediator Service

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.NotificationPublisher = new TaskWhenAllPublisher();
			});
			#endregion

			#region Fluent Validation Service

			services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
			#endregion


			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
