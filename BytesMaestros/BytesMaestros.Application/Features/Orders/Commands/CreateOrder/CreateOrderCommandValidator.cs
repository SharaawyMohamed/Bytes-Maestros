using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.CreateOrder
{
	public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
	{
		public CreateOrderCommandValidator()
		{
			RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.CustomerAddress).NotEmpty();
			RuleFor(x => x.CustomerPhoneNumber).NotEmpty().Matches(@"^\+?[0-9]{7,15}$");
			RuleFor(x => x.OrderTypeId).InclusiveBetween(1, 3);
			RuleFor(x => x.Items).NotEmpty().ForEach(item => item.SetValidator(new OrderItemCommandValidator()));
		}
	}

	public class OrderItemCommandValidator : AbstractValidator<OrderItemCommand>
	{
		public OrderItemCommandValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
			RuleFor(x => x.Quantity).GreaterThan(0);
		}
	}
}

