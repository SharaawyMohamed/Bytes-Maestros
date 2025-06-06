using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Entities
{
	public class Product:BaseEntity<Guid>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string ImageUrl { get; set; } 

		public Type Type { get; set; }
		public int TypeId { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

	}
}
