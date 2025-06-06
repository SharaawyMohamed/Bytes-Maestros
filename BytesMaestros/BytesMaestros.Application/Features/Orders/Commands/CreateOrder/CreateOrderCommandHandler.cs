using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;

namespace BytesMaestros.Application.Features.Orders.Commands.CreateOrder
{
	public class CreateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		public async Task<Response> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
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

			var products = new List<Product>();
			var orderItems = new List<OrderItem>();
			decimal orderTotalAmount = 0;
			
			foreach (var item in request.Items)
			{
				var product = await _unitOfWork.Repository<Guid, Product>().GetByIdAsync(item.ProductId);
				if (product is null)
				{
					return await Response.Fail($"Product with ID {item.ProductId} not found.", HttpStatusCode.NotFound);
				}

				if (product.Stock < item.Quantity)
				{
					return await Response.Fail($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}", HttpStatusCode.BadRequest);
				}
				products.Add(product);

				var orderItem = new OrderItem()
				{
					Id = Guid.NewGuid(),
					OrderId=order.Id,
					ProductId = product.Id,
					Price = product.Price,
					Quantity = item.Quantity,
					
				};
				orderItem.TotalPrice = orderItem.CalculateTotalPrice();
				orderItems.Add(orderItem);

				orderTotalAmount += orderItem.TotalPrice;
			}

			order.OrderItems = orderItems;
			order.TotalAmount=orderTotalAmount;
			await _unitOfWork.Repository<Guid, Order>().AddAsync(order);
			await _unitOfWork.SaveChangesAsync();

			//TODO: Return List Of Time Slots Depend on order
			return await Response.Success(null);


		}
	    
		private async Task<List<DateTimeOffset>> GetTimeSlots(int OrderType)
		{
			return new List<DateTimeOffset>();
		}
	}
}
