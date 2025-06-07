using BytesMaestros.Application.Features.Types.Queries.GetAllTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BytesMaestros.API.Controllers
{
	public class TypeController:BaseAPIController
	{
		private readonly IMediator _mediator ;
		public TypeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetTypes()
		{
			var query = new GetAllTypesQuery();
			return Ok(await _mediator.Send(query));
		}
	}
}
