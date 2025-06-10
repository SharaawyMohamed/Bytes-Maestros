using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard
{
	public class GetCustomerCardQueryValidator:AbstractValidator<GetCustomerCardQuery>
	{
		public GetCustomerCardQueryValidator()
		{
			RuleFor(x => x.Email).NotNull().EmailAddress();
		}
	}
}
