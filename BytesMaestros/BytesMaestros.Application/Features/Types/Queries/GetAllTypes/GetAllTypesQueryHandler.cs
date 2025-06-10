using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;

namespace BytesMaestros.Application.Features.Types.Queries.GetAllTypes
{
	public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork ;
		public GetAllTypesQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
		{
			var types = await _unitOfWork.Repository<int, Type>().GetAllAsync(10,1);
			if (types == null || !types.Any())
			{
				return await Response.Fail("No types found.", HttpStatusCode.NotFound);
			}

			var result=types.Adapt<List<GetAllTypesQueryDto>>();
			return await Response.Success(result, message: "Types retrieved successfully.");

		}
	}
}
