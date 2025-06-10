using BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard
{
	public class GetCustomerCardQueryDto
	{
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public DateTime BestDelivaryTime { get; set; }
		public List<CardOrderDetailsDto> customerOrders { get; set; }=new List<CardOrderDetailsDto> { };

	}
	public record CardOrderDetailsDto(Guid Id,int OrderTypeId,decimal TotalAmount,DateTime? DeliveryTime,string Status,List<OrderItemDto> OrderItems);
}
