using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using OrderEntity = BytesMaestros.Domain.Entities.Order;
using OrderItemEntity = BytesMaestros.Domain.Entities.OrderItem;
using BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard;
using FluentAssertions;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using BytesMaestros.Application.Utility;

namespace BytesMaestros.Test.Order.Queries
{
	public class GetCustomerCardTests : TestBase
	{
		#region Success Senarios
		[Fact]
		public async Task Handle_WhenEnterValidEmailThatHaveOrders_ShouldReturnCustomerCard()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var email = "customer@example.com";

			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var customerCardOrders = new List<OrderEntity>()
			{
				new OrderEntity()
				{
					Id = Guid.NewGuid(),
					CustomerEmail = email,
					CustomerName = "Test Customer",
					CustomerAddress = "123 Test St",
					DeliveryTime = DateTime.Now.AddDays(1),
					Status = "Pending",
					TotalAmount = 100.50m
				},
				new OrderEntity()
				{
					Id = Guid.NewGuid(),
					CustomerEmail = email,
					CustomerName = "Test Customer",
					CustomerAddress = "123 Test St",
					DeliveryTime = DateTime.Now.AddDays(2),
					Status = "Processing",
					TotalAmount = 75.25m
				}
			};

			await AddRangeAsync<OrderEntity, Guid>(customerCardOrders);
			var orderItems = new List<OrderItemEntity>
			{
				new()
				{
					Id = Guid.NewGuid(),
					OrderId = customerCardOrders[0].Id,
					ProductId = products[0].Id,
					ProductName = products[0].Name,
					Quantity = 2,
					Price = products[0].Price,
					TotalPrice = 2 * products[0].Price
				},
				new()
				{
					Id = Guid.NewGuid(),
					OrderId = customerCardOrders[1].Id,
					ProductId = products[1].Id,
					ProductName = products[1].Name,
					Quantity = 1,
					Price = products[1].Price,
					TotalPrice = products[1].Price
				}
			};
			await AddRangeAsync<OrderItemEntity,Guid>(orderItems);
			await unitOfWork.SaveChangesAsync();

			var query = new GetCustomerCardQuery(email);
			var _validator = new GetCustomerCardQueryValidator();
			var handler = new GetCustomerCardQueryHandler(_validator, unitOfWork);

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.OK);

			var response = result.Data as GetCustomerCardQueryDto;
			string customerEmail = response!.CustomerEmail;
			customerEmail.Should().Be(email);

			var customerOrders = response.customerOrders as List<CardOrderDetailsDto>;
			customerOrders.Should().HaveCount(2);

			DateTime bestDeliveryTime = response.BestDelivaryTime;
			bestDeliveryTime.Should().BeAfter(DateTime.Now);
		}
		#endregion

		#region Fail Senarios

		[Fact]
		public async Task Handle_WithInvalidEmailFormat_ShouldReturnBadRequest()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var query = new GetCustomerCardQuery("not-an-email");
			var validator = new GetCustomerCardQueryValidator();
			var handler = new GetCustomerCardQueryHandler(validator, unitOfWork);

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
		
		[Fact]
		public async Task Handle_WithNoActiveOrders_ShouldReturnBadRequest()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var email = "nonexistent@example.com";

			var expiredOrder = new OrderEntity
			{
				Id = Guid.NewGuid(),
				CustomerEmail = email,
				CustomerAddress="Address",
				CustomerName="Name",
				DeliveryTime = DateTime.Now.AddDays(-1) 
			};

			await unitOfWork.Repository<Guid, OrderEntity>().AddAsync(expiredOrder);
			await unitOfWork.SaveChangesAsync();

			var query = new GetCustomerCardQuery(email);
			var validator = new GetCustomerCardQueryValidator();

			var handler = new GetCustomerCardQueryHandler(validator, unitOfWork);

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		#endregion
	}
}
