using BytesMaestros.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BytesMaestros.API.Controllers
{
	public class ProductController:BaseAPIController
	{
		private readonly IMediator _mediator;
		public ProductController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts([FromQuery] GetProductsByTypeQuery query)
		{
			return Ok(await _mediator.Send(query));
		}

	}
}
