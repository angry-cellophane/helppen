using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace HelpPen.Server.Controllers
{
	[AllowAnonymous]
	public class PingController: ApiController
	{
		[HttpGet]
		public HttpResponseMessage Now()
		{
			HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
			responseMessage.Content =new StringContent(DateTimeOffset.Now.ToString());
			responseMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(@"text/plain");

			return responseMessage;
		}
	}
}