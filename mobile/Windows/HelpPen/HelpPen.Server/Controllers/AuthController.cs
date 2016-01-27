using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HelpPen.Server.Model.API;

namespace HelpPen.Server.Controllers
{
	public class AuthController : ApiController
	{
		private readonly static RNGCryptoServiceProvider _rngCryptoServiceProvider;

		static AuthController()
		{
			_rngCryptoServiceProvider = new RNGCryptoServiceProvider();
		}
		[HttpGet]
		public string Test()
		{
			return DateTimeOffset.Now.ToString();
		}

		[HttpPost]
		public async Task<IHttpActionResult> Login([FromBody] Credentials credentials, CancellationToken cancellationToken)
		//public async Task<IHttpActionResult> Login([FromUri]string userName, [FromUri] string password, CancellationToken cancellationToken)
		{
			bool isAuthentificated = await CheckPassword(credentials, cancellationToken);
			//bool isAuthentificated = await CheckPassword(new Credentials {UserName = "", Password = ""}, cancellationToken);

			IHttpActionResult actionResult;

			if (isAuthentificated)
			{
				byte[] sessionData = new byte[64];

				_rngCryptoServiceProvider.GetNonZeroBytes(sessionData);

				string sessionId = BitConverter.ToString(sessionData).Replace("-", "").ToLowerInvariant();

				HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

				CookieHeaderValue cookie = new CookieHeaderValue(@"sessionId", sessionId) {Expires = DateTimeOffset.Now + TimeSpan.FromHours(1)};

				responseMessage.Headers.AddCookies(new[] { cookie });

				actionResult = new ResponseMessageResult(responseMessage);
			}
			else
			{
				actionResult = Unauthorized();
			}

			return actionResult;
		}

		private async Task<bool> CheckPassword(Credentials credentials, CancellationToken cancellationToken)
		{
			return string.Compare(credentials.UserName, credentials.Password, StringComparison.InvariantCultureIgnoreCase) == 0;
		}
	}
}