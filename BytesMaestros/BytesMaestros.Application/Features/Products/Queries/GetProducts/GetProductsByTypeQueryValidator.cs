using FluentValidation;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Products.Queries.GetProducts
{
	public class GetProductsByTypeQueryValidator:AbstractValidator<GetProductsByTypeQuery>
	{
		public GetProductsByTypeQueryValidator()
		{
			RuleFor(x => x.TypeId).NotNull().GreaterThan(0);
			RuleFor(x => x.pageSize).NotNull().GreaterThan(0);
			RuleFor(x => x.pageIndex).NotNull().GreaterThan(0);
		}
	}
}
