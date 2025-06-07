using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.ScheduleOrderDelivery
{
	public class ScheduleOrderDeliveryCommandValidator:AbstractValidator<ScheduleOrderDeliveryCommand>
	{
		public ScheduleOrderDeliveryCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
			RuleFor(x => x.DeliveryDate).GreaterThan(DateTimeOffset.UtcNow);
		}
	}
}
