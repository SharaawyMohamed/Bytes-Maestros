
using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Products.Queries.GetProducts
{
	public class GetProductsByTypeQueryHandler : IRequestHandler<GetProductsByTypeQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidator<GetProductsByTypeQuery> _validator;
		public GetProductsByTypeQueryHandler(IUnitOfWork unitOfWork,IValidator<GetProductsByTypeQuery> validator)
		{
			_unitOfWork = unitOfWork;
			_validator = validator;
		}
		public async Task<Response> Handle(GetProductsByTypeQuery request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request, cancellationToken);
			if(!validationResult.IsValid)
			{
				return await Response.Fail("Validation failed.", HttpStatusCode.BadRequest, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
			}

			var products = await _unitOfWork.Repository<Guid, Product>().GetWithPrdicateAsync(x => x.TypeId == request.TypeId, request.pageSize, request.pageIndex);
			if (products == null || !products.Any())
			{
				return await Response.Fail("No products found of this type!", HttpStatusCode.NotFound);
			}
			var result = products.Adapt<List<GetProductsByTypeQueryDto>>();
			return await Response.Success(result, "Products retrieved successfully!");
		}
	}
}
