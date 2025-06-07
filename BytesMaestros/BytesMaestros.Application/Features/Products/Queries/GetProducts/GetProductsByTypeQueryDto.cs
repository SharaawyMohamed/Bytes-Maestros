using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Products.Queries.GetProducts
{
	public record GetProductsByTypeQueryDto(
		Guid Id,
		string Name,
		decimal Price,
		int Stock,
		string Description,
		string ImageUrl
	);

}
