using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.RescheduelCustomerOrders
{
	public class RescheduleCustomerOrdersCommandvalidator:AbstractValidator<RescheduleCustomerOrdersCommand>
	{
		public RescheduleCustomerOrdersCommandvalidator()
		{
			RuleFor(x => x.customerEmail).NotEmpty().EmailAddress();
			RuleFor(x => x.greenDelivaryTime).NotNull().GreaterThan(DateTime.Now);
		}
	}
}
