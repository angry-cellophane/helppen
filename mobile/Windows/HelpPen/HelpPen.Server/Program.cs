using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace HelpPen.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			using (WebApp.Start<Startup>(ConfigurationManager.AppSettings[@"BindAddress"]))
			{
				Console.ReadLine();
			}
		}
	}
}
