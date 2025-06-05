using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Entities
{
	public class Order : BaseEntity<Guid>
	{
		public decimal TotalAmount { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerAddress { get; set; }
		public string Status { get; set; } = "Pending";
		public DateTime DeliveryTime { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

		public decimal CalculateTotalAmount()
		=> OrderItems.Sum(item => item.TotalPrice);
	}

}
