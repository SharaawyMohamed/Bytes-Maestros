using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Commands.RescheduelCustomerOrders
{
	public class RescheduleCustomerOrdersCommandHandler : IRequestHandler<RescheduleCustomerOrdersCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidator<RescheduleCustomerOrdersCommand> _validator;
		public RescheduleCustomerOrdersCommandHandler(IUnitOfWork unitOfWork, IValidator<RescheduleCustomerOrdersCommand> validator)
		{
			_unitOfWork = unitOfWork;
			_validator = validator;
		}

		public async Task<Response> Handle(RescheduleCustomerOrdersCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request);
			if (!validationResult.IsValid)
			{
				return await Response.Fail("Validation failed.",HttpStatusCode.BadRequest, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
			}

			var orders = await _unitOfWork.Repository<Guid, Order>().GetWithPrdicateAsync(x => x.CustomerEmail == request.customerEmail && x.DeliveryTime > DateTime.Now, 100, 1);
			if(orders is null || orders.Count == 0)
			{
				return await Response.Fail("You not have orders in you card!", HttpStatusCode.BadRequest);  
			}

			foreach (var order in orders!)
			{
				order.DeliveryTime=request.greenDelivaryTime;
			}
			await _unitOfWork.SaveChangesAsync();

			return await Response.Success("Congrats🎉🎉, Your Orders Rescheduled successfully.");
		}
	}
}
