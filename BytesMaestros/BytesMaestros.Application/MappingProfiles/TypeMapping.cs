using BytesMaestros.Application.Features.Types.Queries.GetAllTypes;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BytesMaestros.Domain.Entities.Type;
namespace BytesMaestros.Application.MappingProfiles
{
	public class TypeMapping : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Type, GetAllTypesQueryDto>();
		}
	}
}
