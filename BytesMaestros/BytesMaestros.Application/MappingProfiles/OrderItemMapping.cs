using BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery;
using BytesMaestros.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.MappingProfiles
{
	public class OrderItemMapping : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<OrderItem,OrderItemDto>();
		}
	}
}
