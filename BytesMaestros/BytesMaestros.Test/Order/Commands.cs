using Bogus.DataSets;
using BytesMaestros.Application.Features.Orders.Commands.CreateOrder;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using OrderEntity = BytesMaestros.Domain.Entities.Order;

namespace BytesMaestros.Test.Order
{
	public class Commands : TestBase
	{
		#region Success Senario
		[Fact]
		public async Task WhenOrderItemsCreatedSuccessfully_ShouldGetSuccessResponse()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"),3),
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111115"),1)
			};
			var command = new CreateOrderCommand("Sharawy", "sharawy@gmail.com", "Cairo,Egypt", "+20112334454", 1, items);

			var validator = new CreateOrderCommandValidator();

			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);
			//Act

			var result = await handler.Handle(command, CancellationToken.None);

			//Assert

			result.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Data.Should().NotBeNull();
			result.Word.Should().NotBeNull();
			var data = result.Data as List<DateTime>;
			data.Count.Should().BeGreaterThan(0);

		}
		#endregion

		#region Failed Senario
		[Fact]
		public async Task WhenValidationFialed_ShouldReturnValidationError()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111113"), 1)
			};
			var validator = new CreateOrderCommandValidator();
			var command = new CreateOrderCommand("Name", "invalid-email", "Address", "+2089", 1, items);

			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);
			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert

			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			var errors = result.Errors as List<string>;
			errors.Should().NotBeNullOrEmpty();
			errors.Count().Should().BeGreaterThan(0);
		}
		[Fact]
		public async Task WhenOrderTypeNotFound_ShouldReturnNotFoundResponse()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), 1),
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111114"), 3),
			};
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 5, items);
			var validator = new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task WhenProductNotFound_ShouldReturnNotFoundResponse()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var invalidProductId = Guid.NewGuid();
			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(invalidProductId, 1)
			};
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 1, items);
			var validator = new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task WhenProductDoesNotBelongToOrderType_ShouldReturnNotFoundResponse()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("22222222-2222-2222-2222-222222222221"), 1)
			};
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 1, items);
			var validator=new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task WhenInsufficientStock_ShouldReturnBadRequestResponse()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), 500) // Stock is 150
            };
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 1, items);
			var validator = new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
		#endregion

		#region Arithmatic Logic Cases
		[Fact]
		public async Task WhenMultipleProductsOrdered_ShouldCalculateCorrectTotalAmount()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), 2), 
                new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111112"), 1)
            };
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 1, items);
			var vaidator = new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, vaidator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			var order = await _unitOfWork.Repository<Guid, OrderEntity>().GetByIdAsync((Guid)result!.Word!);
			order.Should().NotBeNull();
			order!.TotalAmount.Should().Be(96.98m);
		}

		[Fact]
		public async Task WhenOrderCreated_ShouldReduceProductStock()
		{
			// Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);

			var initialStock = (await _unitOfWork.Repository<Guid, ProductEntity>().GetByIdAsync(Guid.Parse("11111111-1111-1111-1111-111111111111")))!.Stock;

			var items = new List<OrderItemCommand>()
			{
				new OrderItemCommand(Guid.Parse("11111111-1111-1111-1111-111111111111"), 100)
			};
			var command = new CreateOrderCommand("Name", "test@email.com", "Address", "+20123456789", 1, items);
			var validator = new CreateOrderCommandValidator();
			var handler = new CreateOrderCommandHandler(_unitOfWork, validator);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			var product = await _unitOfWork.Repository<Guid, ProductEntity>().GetByIdAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"));
			product.Should().NotBeNull();
			product.Stock.Should().Be(50);
		}
		#endregion
	}
}
