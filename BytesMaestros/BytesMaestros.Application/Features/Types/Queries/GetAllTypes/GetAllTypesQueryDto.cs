using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Types.Queries.GetAllTypes
{
    public record GetAllTypesQueryDto(
		Guid Id,
		string Name,
		string Description,
		string ImageUrl
	);
}
