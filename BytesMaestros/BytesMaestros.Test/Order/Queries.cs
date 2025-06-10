using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using OrderEntity = BytesMaestros.Domain.Entities.Order;
using OrderItemEntity = BytesMaestros.Domain.Entities.OrderItem;

namespace BytesMaestros.Test.Order
{
	public class Queries:TestBase
	{
		#region MyRegion
		[Fact]
		public async Task Handle_WhenGetCustomerCardSuccessfully_ShouldGetSuccessResponse()
		{
			var _unitOfWOrk = GetUnitOfWork();

			var types = await HardData.GetTypesAsync();
			await AddRangeAsync<TypeEntity, int>(types);

			var products=await HardData.GetProductsAsync();
			await AddRangeAsync<ProductEntity,Guid>(products); 

			//var orderItems=new List<OrderItemEntity>() { 
			//	new OrderItemEntity()

			//}
			//var order=new OrderEntity()
			//{

			//}
		}
		#endregion
	}
}
