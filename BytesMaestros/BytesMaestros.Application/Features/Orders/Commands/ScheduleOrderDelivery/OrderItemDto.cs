using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery
{
	public record OrderItemDto(
		Guid Id,
		Guid OrderId,
		string ProductName,
		decimal TotalPrice,
		int Quantity
	);
}


