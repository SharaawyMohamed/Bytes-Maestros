using BytesMaestros.Application.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Types.Queries.GetAllTypes
{
	public record GetAllTypesQuery(int pageSize, int pageIndex) :IRequest<Response>;
}
