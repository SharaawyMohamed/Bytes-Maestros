using BytesMaestros.Application.Features.Products.Queries.GetProducts;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Persistence.Data;
using FluentAssertions;
using Mapster;
using Microsoft.EntityFrameworkCore.InMemory.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
namespace BytesMaestros.Test.Product
{
	public class Queries:TestBase
	{
		#region Success Senario
		[Fact]
		public async Task Handle_WhenGetProductsSuccessfully_ShouldGetSuccessResponse()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products =await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);


			var query = new GetProductsByTypeQuery(1, 5, 1);
			var validator = new GetProductsByTypeQueryValidator();
			var handler = new GetProductsByTypeQueryHandler(_unitOfWork, validator);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Data.Should().NotBeNull();
			var data = result.Data as List<GetProductsByTypeQueryDto>;
			data.Should().NotBeNull();
			data.Count.Should().Be(5);

		}
		#endregion
		[Fact]
		public async Task Handle_WhenValidationFails_ShouldGetFaildResponse()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);


			var query = new GetProductsByTypeQuery(0, 5, -1);
			var validator = new GetProductsByTypeQueryValidator();
			var handler = new GetProductsByTypeQueryHandler(_unitOfWork, validator);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			result.Data.Should().BeNull();

		}

		[Fact]
		public async Task Handle_WhenNotDataRetrived_ShouldGetFaildRespose()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products = await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity, Guid>(products);


			var query = new GetProductsByTypeQuery(2, 10,2 );
			var validator = new GetProductsByTypeQueryValidator();
			var handler = new GetProductsByTypeQueryHandler(_unitOfWork, validator);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
			result.Data.Should().BeNull();

		}
		#region Failed Senario


		#endregion
	}
}
