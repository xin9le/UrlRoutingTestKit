using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SampleApp
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute
			(
				name:	  "Diary",
				url:	  "{controller}/{action}/{year}/{month}/{day}",
				defaults: new { controller = "Person", action = "Diary" }
			);
		}
	}
}