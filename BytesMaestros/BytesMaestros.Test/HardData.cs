using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeEntity = BytesMaestros.Domain.Entities.Type;
using ProductEntity = BytesMaestros.Domain.Entities.Product;
namespace BytesMaestros.Test
{
	public class HardData
	{
		public static async Task<List<TypeEntity>> GetTypesAsync()
		{
			var types= new List<TypeEntity>()
						{
							new TypeEntity()
							{
								Name = "In-Stock Products",
								Description = "Items available in our warehouse. Eligible for same-day delivery if ordered before 18:00.",
								ImageUrl = "empty",

							},
							new TypeEntity()
							{
								Name = "Fresh Food",
								Description = "Products prepared daily (e.g., bakery goods, ready meals). Eligible for same-day delivery if ordered before 12:00.",
								ImageUrl = "empty",

							},
							new TypeEntity()
							{
								Name = "External Products",
								Description = "Supplied by third-party partners. These must be ordered at least 3 days in advance.",
								ImageUrl = "empty",

							}
						};
			return await Task.FromResult(types);
		}

		public static async Task<List<ProductEntity>> GetProductsAsync()
		{
			var products= new List<ProductEntity>
					{
						new ProductEntity
						{
							Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
							Name = "Wireless Mouse",
							Description = "Ergonomic wireless mouse.",
							Price = 25.99m,
							Stock = 150,
							ImageUrl = "https://example.com/images/mouse.jpg",
							TypeId = 1
						},
						new ProductEntity
						{
							Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
							Name = "USB Keyboard",
							Description = "Mechanical USB keyboard.",
							Price = 45.00m,
							Stock = 80,
							ImageUrl = "https://example.com/images/keyboard.jpg",
							TypeId = 1
						},
						new ProductEntity
						{
							Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
							Name = "HDMI Cable",
							Description = "High-speed HDMI cable.",
							Price = 10.50m,
							Stock = 200,
							ImageUrl = "https://example.com/images/hdmi.jpg",
							TypeId = 1
						},
						new ProductEntity
						{
							Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
							Name = "Laptop Stand",
							Description = "Adjustable laptop stand.",
							Price = 30.75m,
							Stock = 60,
							ImageUrl = "https://example.com/images/laptop-stand.jpg",
							TypeId = 1
						},
						new ProductEntity
						{
							Id = Guid.Parse("11111111-1111-1111-1111-111111111115"),
							Name = "Webcam",
							Description = "1080p HD webcam.",
							Price = 55.25m,
							Stock = 40,
							ImageUrl = "https://example.com/images/webcam.jpg",
							TypeId = 1
						},
						new ProductEntity
						{
							Id = Guid.Parse("22222222-2222-2222-2222-222222222221"),
							Name = "Fresh Bread",
							Description = "Daily baked bread.",
							Price = 3.99m,
							Stock = 120,
							ImageUrl = "https://example.com/images/bread.jpg",
							TypeId = 2
						},
						new ProductEntity
						{
							Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
							Name = "Organic Milk",
							Description = "Fresh organic milk.",
							Price = 4.50m,
							Stock = 100,
							ImageUrl = "https://example.com/images/milk.jpg",
							TypeId = 2
						},
						new ProductEntity
						{
							Id = Guid.Parse("22222222-2222-2222-2222-222222222223"),
							Name = "Free-range Eggs",
							Description = "Dozen free-range eggs.",
							Price = 5.25m,
							Stock = 90,
							ImageUrl = "https://example.com/images/eggs.jpg",
							TypeId = 2
						},
						new ProductEntity
						{
							Id = Guid.Parse("22222222-2222-2222-2222-222222222224"),
							Name = "Fresh Salad Mix",
							Description = "Mixed greens for salads.",
							Price = 6.99m,
							Stock = 75,
							ImageUrl = "https://example.com/images/salad.jpg",
							TypeId = 2
						},
						new ProductEntity
						{
							Id = Guid.Parse("22222222-2222-2222-2222-222222222225"),
							Name = "Ready Meal",
							Description = "Prepared ready-to-eat meal.",
							Price = 8.50m,
							Stock = 50,
							ImageUrl = "https://example.com/images/ready-meal.jpg",
							TypeId = 2
						},
						new ProductEntity
						{
							Id = Guid.Parse("33333333-3333-3333-3333-333333333331"),
							Name = "Imported Cheese",
							Description = "Aged imported cheese.",
							Price = 15.00m,
							Stock = 25,
							ImageUrl = "https://example.com/images/cheese.jpg",
							TypeId = 3
						},
						new ProductEntity
						{
							Id = Guid.Parse("33333333-3333-3333-3333-333333333332"),
							Name = "Exotic Spices",
							Description = "Spices from around the world.",
							Price = 20.00m,
							Stock = 30,
							ImageUrl = "https://example.com/images/spices.jpg",
							TypeId = 3
						},
						new ProductEntity
						{
							Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
							Name = "Gourmet Chocolate",
							Description = "Premium imported chocolate.",
							Price = 25.00m,
							Stock = 40,
							ImageUrl = "https://example.com/images/chocolate.jpg",
							TypeId = 3
						},
						new ProductEntity
						{
							Id = Guid.Parse("33333333-3333-3333-3333-333333333334"),
							Name = "Imported Olive Oil",
							Description = "Extra virgin olive oil.",
							Price = 18.00m,
							Stock = 35,
							ImageUrl = "https://example.com/images/olive-oil.jpg",
							TypeId = 3
						},
						new ProductEntity
						{
							Id = Guid.Parse("33333333-3333-3333-3333-333333333335"),
							Name = "Specialty Coffee",
							Description = "Imported coffee beans.",
							Price = 22.00m,
							Stock = 45,
							ImageUrl = "https://example.com/images/coffee.jpg",
							TypeId = 3
						}
					};
			return await Task.FromResult(products);
		}
	}
}
