using BytesMaestros.Application.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.RescheduelCustomerOrders
{
	public record RescheduleCustomerOrdersCommand(string customerEmail,DateTime greenDelivaryTime):IRequest<Response>;
	
}
