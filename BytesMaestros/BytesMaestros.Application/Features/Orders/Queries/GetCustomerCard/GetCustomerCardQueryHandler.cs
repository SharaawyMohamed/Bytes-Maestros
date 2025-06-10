using BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery;
using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard
{
	public class GetCustomerCardQueryHandler : IRequestHandler<GetCustomerCardQuery, Response>
	{
		private readonly IValidator<GetCustomerCardQuery> _validator;
		private readonly IUnitOfWork _unitOfWork;
		public GetCustomerCardQueryHandler(IValidator<GetCustomerCardQuery> validator, IUnitOfWork unitOfWork)
		{
			_validator = validator;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetCustomerCardQuery request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request, cancellationToken);
			if (!validationResult.IsValid)
			{
				return await Response.Fail("Vlidation Error", HttpStatusCode.BadRequest, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
			}

			var customerCard = (await _unitOfWork.Repository<Guid, Order>().GetWithPrdicateAsync(x => x.CustomerEmail == request.Email && x.DeliveryTime > DateTime.Now, 5, 1))!.ToList();
			if (!customerCard!.Any())
			{
				return await Response.Fail("You Card Is Empty!", HttpStatusCode.BadRequest);
			}

			var response = new GetCustomerCardQueryDto();

			response.CustomerEmail = customerCard[0].CustomerEmail;
			response.CustomerAddress = customerCard[0].CustomerAddress;
			response.CustomerName = customerCard[0].CustomerName;

			var bestTimeSlot = DateTime.Now;
			foreach (var i in customerCard)
			{
				var cardOrderItems = await _unitOfWork.Repository<Guid, OrderItem>().GetWithPrdicateAsync(x => (x.OrderId == i.Id), 100, 1);
				if (cardOrderItems == null || !cardOrderItems.Any())
				{
					continue;
				}
				var items = cardOrderItems.Adapt<List<OrderItemDto>>();

				var product = await _unitOfWork.Repository<Guid, Product>().GetEntityWithPrdicateAsync(x => x.Id == cardOrderItems![0].ProductId);

				if (product == null)
					continue;

				var slot = (await SlotsGenerator.GenerateTimeSlots(product!.TypeId)).FirstOrDefault();

				if (slot > bestTimeSlot)
					bestTimeSlot = slot;

				var orderDetails = new CardOrderDetailsDto(
						i.Id,
						product.TypeId,
						i.TotalAmount,
						i.DeliveryTime,
						i.Status,
						items
					);
				response.customerOrders.Add(orderDetails);
			}
			response.BestDelivaryTime = bestTimeSlot;

			return await Response.Success(response, message: "Customer Orders with best Delivary Schedule Time");

		}
	}
}
