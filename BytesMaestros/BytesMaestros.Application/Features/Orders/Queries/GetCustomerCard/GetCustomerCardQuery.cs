using BytesMaestros.Application.Utility;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Features.Orders.Queries.GetCustomerCard
{
	public record GetCustomerCardQuery(string Email):IRequest<Response>;
	
}
