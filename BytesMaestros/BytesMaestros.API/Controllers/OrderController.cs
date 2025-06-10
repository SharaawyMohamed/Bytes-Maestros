using BytesMaestros.Application.Features.Orders.Commands.CreateOrder;
using BytesMaestros.Application.Features.Orders.Commands.RescheduelCustomerOrders;
using BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery;
using BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BytesMaestros.API.Controllers
{
	public class OrderController : BaseAPIController
	{
		private readonly IMediator _mediator;
		public OrderController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
		{
			return Ok(await _mediator.Send(command));
		}

		[HttpPost("schedule-delivery")]
		public async Task<IActionResult> ScheduleDelivery([FromBody] ScheduleOrderDeliveryCommand command)
		{
			return Ok(await _mediator.Send(command));
		}

		[HttpGet("get-customer-orders")]
		public async Task<IActionResult> GetCustomerOrders(string customerEmail)
		{
			var query=new GetCustomerCardQuery(customerEmail);
			return Ok(await _mediator.Send(query));
		}

		[HttpPost("reschedule-orders-with-green-delivery")]
		public async Task<IActionResult> RescheduleOrders([FromBody] RescheduleCustomerOrdersCommand command)
		{
			return Ok(await _mediator.Send(command));
		}
	}
}
