using BytesMaestros.Application.Features.Types.Queries.GetAllTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BytesMaestros.API.Controllers
{
	public class TypeController(IMediator mediator):BaseAPIController
	{
		private readonly IMediator _mediator = mediator;

		[HttpGet]
		public async Task<IActionResult> GetTypes(GetAllTypesQuery query)
		{
			return Ok(await _mediator.Send(query));
		}
	}
}
