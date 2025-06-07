using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Utility
{
	public class Response
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
		public Guid? Word { get; set; } 
		public List<string> Errors { get; set; } = new List<string>();

		public static async Task<Response> Success(object data,Guid? word=null, string message = null)
		{
			return new Response
			{
				StatusCode = HttpStatusCode.OK,
				Message = message,
				Data = data,
				Word = word
			};
		}

		public static async Task<Response> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, List<string> errors = null)
		{
			return new Response
			{
				StatusCode = statusCode,
				Message = message,
				Errors = errors
			};
		}
	}
}
