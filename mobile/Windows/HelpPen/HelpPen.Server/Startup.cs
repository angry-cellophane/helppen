using System.Web.Http;
using Owin;

namespace HelpPen.Server
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			HttpConfiguration config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}"
			);

			appBuilder.UseWebApi(config);
		}
	}
}