using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery
{
	public class ScheduleOrderDeliveryCommandHandler : IRequestHandler<ScheduleOrderDeliveryCommand, Response>
	{
		private readonly IValidator<ScheduleOrderDeliveryCommand> _validator;
		private readonly IUnitOfWork _unitOfWork;
		public ScheduleOrderDeliveryCommandHandler(IUnitOfWork unitOfWork, IValidator<ScheduleOrderDeliveryCommand> validator)
		{
			_unitOfWork = unitOfWork;
			_validator = validator;
		}
		public async Task<Response> Handle(ScheduleOrderDeliveryCommand request, CancellationToken cancellationToken)
		{
			var validationResult = _validator.Validate(request);
			if (!validationResult.IsValid)
			{
				return await Response.Fail("Validation failed.", System.Net.HttpStatusCode.BadRequest, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
			}

			var order = await _unitOfWork.Repository<Guid, Order>().GetByIdAsync(request.OrderId);
			if (order == null )
			{
				return await Response.Fail("Order not found!", System.Net.HttpStatusCode.NotFound);
			}

			var timeSlot = request.DeliveryDate;
			order.DeliveryTime = new DateTime(timeSlot.Year,timeSlot.Month,timeSlot.Day,timeSlot.Hour,0,0);
			await _unitOfWork.SaveChangesAsync();

			var orderItems=await _unitOfWork.Repository<Guid,OrderItem>().GetWithPrdicateAsync(x => x.OrderId == request.OrderId, 50, 1);
		

			var orderDetails=order.Adapt<OrderDetailsDto>();

			var mappedItems=orderItems.Adapt<IEnumerable<OrderItemDto>>();
			orderDetails.OrderItems = mappedItems;
			return await Response.Success(orderDetails,message: "Order delivery scheduled successfully!");
		}
	}

}
