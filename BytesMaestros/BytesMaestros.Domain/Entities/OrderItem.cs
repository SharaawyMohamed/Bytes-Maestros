﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Entities
{
	public class OrderItem : BaseEntity<Guid>
	{
		public Guid OrderId { get; set; }
		public Order Order { get; set; }

		public Guid ProductId { get; set; }
		public Product Product { get; set; }

		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal TotalPrice { get; set; }

		public decimal CalculateTotalPrice()
			=> Quantity * Price;

	}
}
