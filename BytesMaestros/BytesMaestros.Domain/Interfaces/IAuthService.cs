using BytesMaestros.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Domain.Interfaces
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(Customer user, UserManager<Customer> _userManager);
	}
}
