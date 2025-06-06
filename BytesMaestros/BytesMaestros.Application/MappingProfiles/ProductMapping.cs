using BytesMaestros.Application.Features.Products.Queries.GetProducts;
using BytesMaestros.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.MappingProfiles
{
	public class ProductMapping : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Product, GetProductsByTypeQueryDto>();
		}
	}
}
