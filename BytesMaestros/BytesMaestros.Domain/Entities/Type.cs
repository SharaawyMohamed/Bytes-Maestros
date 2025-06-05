using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Entities
{
	public class Type:BaseEntity<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
