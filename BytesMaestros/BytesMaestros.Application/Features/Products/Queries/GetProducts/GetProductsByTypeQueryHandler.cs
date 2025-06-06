
using BytesMaestros.Application.Utility;
using BytesMaestros.Domain.Entities;
using BytesMaestros.Domain.Repositories;
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
	public class GetProductsByTypeQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductsByTypeQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		public async Task<Response> Handle(GetProductsByTypeQuery request, CancellationToken cancellationToken)
		{
			var products = await _unitOfWork.Repository<Guid, Product>().GetWithPrdicateAsync(x => x.TypeId == request.Id, request.pageSize, request.pageIndex);
			if (products == null || !products.Any())
			{
				return await Response.Fail("No products found of this type!", HttpStatusCode.NotFound);
			}
			var result = products.Adapt<List<GetProductsByTypeQueryDto>>();
			return await Response.Success(result, "Products retrieved successfully!");
		}
	}
}
