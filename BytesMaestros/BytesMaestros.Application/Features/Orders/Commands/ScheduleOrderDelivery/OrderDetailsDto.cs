using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery
{
	public class OrderDetailsDto
	{
		public Guid Id { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public decimal TotalAmount { get; set; }
		public DateTime DeliveryTime { get; set; }
		public string Status { get; set; }
		public IEnumerable<OrderItemDto> OrderItems { get; set; }
	}

}
