using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;

namespace BytesMaestros.Application.Features.Orders.Commands.CreateOrder
{
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidator<CreateOrderCommand> _validator;
		public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateOrderCommand> validator)
		{
			_unitOfWork = unitOfWork;
			_validator = validator;
		}
		public async Task<Response> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request, cancellationToken);
			if (!validationResult.IsValid)
			{
				return await Response.Fail("Validation failed.", HttpStatusCode.BadRequest, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
			}

			var OrderType = await _unitOfWork.Repository<int, Type>().GetByIdAsync(request.OrderTypeId);
			if (OrderType == null)
			{
				return await Response.Fail("Order type not found.", HttpStatusCode.NotFound);
			}

			var order = new Order()
			{
				Id = Guid.NewGuid(),
				CustomerAddress = request.CustomerAddress,
				CustomerEmail = request.CustomerEmail,
				CustomerName = request.CustomerName,
			};

			var orderItems = new List<OrderItem>();
			decimal orderTotalAmount = 0;

			foreach (var item in request.Items)
			{
				var product = await _unitOfWork.Repository<Guid, Product>().GetEntityWithPrdicateAsync(x => x.TypeId == request.OrderTypeId && x.Id == item.ProductId);
				if (product is null)
				{
					return await Response.Fail($"Product with ID {item.ProductId} not found or it's not belongs to the type with Id:{request.OrderTypeId} !", HttpStatusCode.NotFound);
				}

				if (product.Stock < item.Quantity)
				{
					return await Response.Fail($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}", HttpStatusCode.BadRequest);
				}
				else
				{
					product.Stock -= item.Quantity;
				}

				var orderItem = new OrderItem()
				{
					Id = Guid.NewGuid(),
					OrderId = order.Id,
					ProductName = product.Name,
					ProductId = product.Id,
					Price = product.Price,
					Quantity = item.Quantity,

				};
				orderItem.TotalPrice = orderItem.CalculateTotalPrice();
				orderItems.Add(orderItem);

				orderTotalAmount += orderItem.TotalPrice;
			}

			order.OrderItems = orderItems;
			order.TotalAmount = orderTotalAmount;

			var slots = await SlotsGenerator.GenerateTimeSlots(request.OrderTypeId);
			order.DeliveryTime = slots.FirstOrDefault();

			
			await _unitOfWork.Repository<Guid, Order>().AddAsync(order);
			await _unitOfWork.SaveChangesAsync();

			return await Response.Success(slots, order.Id, "Order created successfully.Please select a delivery time slot.");
		}
	}
}
