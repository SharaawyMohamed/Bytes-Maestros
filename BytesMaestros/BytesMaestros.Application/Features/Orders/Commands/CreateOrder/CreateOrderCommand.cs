using BytesMaestros.Application.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.CreateOrder
{
	public record CreateOrderCommand(string CustomerName, string CustomerEmail, string CustomerAddress, string CustomerPhoneNumber,int OrderTypeId, List<OrderItemCommand> Items) : IRequest<Response>;

	public record OrderItemCommand(Guid ProductId, int Quantity);
}
