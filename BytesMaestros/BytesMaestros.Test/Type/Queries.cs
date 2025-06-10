using BytesMaestros.Application.Features.Orders.Commands.CreateOrder;
using BytesMaestros.Application.Features.Types.Queries.GetAllTypes;
using BytesMaestros.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
namespace BytesMaestros.Test.Type
{
	public class Queries:TestBase
	{
		#region Success Senario
		[Fact]
		public async Task Handle_WhenGetAllTypesSuccessfully_ShouldGetSuccessResponse()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();
			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity,int>(types);
			var query = new GetAllTypesQuery();
			var handler = new GetAllTypesQueryHandler(_unitOfWork);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Data.Should().NotBeNull();
			var data = result.Data as List<GetAllTypesQueryDto>;
			data.Count.Should().Be(3);
		}
		#endregion

		#region Failed Senario
		[Fact]
		public async Task Handle_WhenNoTypesFound_ShouldGetFiledResponse()
		{
			//Arrange
			var _unitOfWork = GetUnitOfWork();

			var query = new GetAllTypesQuery();
			var handler = new GetAllTypesQueryHandler(_unitOfWork);

			//Act
			var result = await handler.Handle(query, CancellationToken.None);

			//Assert
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
			result.Data.Should().BeNull();
		}
		#endregion
	}
}
