using BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using OrderEntity = BytesMaestros.Domain.Entities.Order;
using OrderItemEntity = BytesMaestros.Domain.Entities.OrderItem;
namespace BytesMaestros.Test.Order.Commands
{
	public class ScheduleOrderDeliveryTests : TestBase
	{
		private readonly ScheduleOrderDeliveryCommandValidator _validator = new();

		#region Success Scenario
		[Fact]
		public async Task Handle_WhenScheduleOrderSuccessfully_ShouldGetSuccessResponse()
		{ 
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var order = new OrderEntity()
			{
				Id = Guid.NewGuid(),
				CustomerAddress = "Cairo, Egypt",
				CustomerEmail = "customer@gmail.com",
				CustomerName = "customer",
				Status = "Pending",
				DeliveryTime = DateTime.Now.AddDays(1),
			};

			var items = new List<OrderItemEntity>();
			for (int i = 0; i < 3; i++)
			{
				var item = products[i];
				items.Add(new OrderItemEntity()
				{
					Id = Guid.NewGuid(),
					OrderId = order.Id,
					ProductId = item.Id,
					ProductName = item.Name,
					Quantity = 2,
					TotalPrice = 2 * item.Price,
					Price = item.Price
				});
			}
			order.OrderItems = items;
			await unitOfWork.Repository<Guid, OrderEntity>().AddAsync(order);
			await unitOfWork.SaveChangesAsync();

			var command = new ScheduleOrderDeliveryCommand(order.Id, DateTime.Now.AddHours(4));
			var handler = new ScheduleOrderDeliveryCommandHandler(unitOfWork, _validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.Data.Should().NotBeNull();
			var data = result.Data as OrderDetailsDto;
			Guid orderId = data.Id;
			orderId.Should().Be(order.Id);
			IEnumerable<OrderItemDto> orderItems = data.OrderItems;
			orderItems.Should().HaveCount(3);
		}
		#endregion

		#region Validation Failure Scenarios
		[Fact]
		public async Task Handle_WithEmptyOrderId_ShouldReturnBadRequest()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var command = new ScheduleOrderDeliveryCommand(Guid.Empty, DateTime.Now.AddHours(2));
			var handler = new ScheduleOrderDeliveryCommandHandler(unitOfWork, _validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Handle_WithPastDeliveryDate_ShouldReturnBadRequest()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var command = new ScheduleOrderDeliveryCommand(Guid.NewGuid(), DateTime.Now.AddHours(-1));
			var handler = new ScheduleOrderDeliveryCommandHandler(unitOfWork, _validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			var errors = result.Errors as List<string>;
			errors.Count.Should().BeGreaterThan(0);
		}
		
		[Fact]
		public async Task Handle_WithNonExistentOrder_ShouldReturnNotFound()
		{
			// Arrange
			var unitOfWork = GetUnitOfWork();
			var nonExistentOrderId = Guid.NewGuid();
			var command = new ScheduleOrderDeliveryCommand(nonExistentOrderId, DateTime.Now.AddHours(4));
			var handler = new ScheduleOrderDeliveryCommandHandler(unitOfWork, _validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
			result.Message.Should().Be("Order not found!");
		}


		
		#endregion
	}
}