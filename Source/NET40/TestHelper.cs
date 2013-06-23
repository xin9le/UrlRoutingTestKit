using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;



namespace UrlRoutingTestKit
{
	/// <summary>
	/// Provides the URL routing test helpers.
	/// </summary>
	public static class TestHelper
	{
		#region Dynamic
		/// <summary>
		/// Tests the specified URL.
		/// </summary>
		/// <typeparam name="T">Expected controller type.</typeparam>
		/// <param name="url">URL to be verified.</param>
		/// <param name="action">Expected action name.</param>
		/// <returns>Parameters to be verified.</returns>
		public static dynamic MapTo<T>(this string url, string action)
			where T : Controller
		{
			return url.Route().MapTo<T>(action);
		}
	
		
		/// <summary>
		/// Tests the specified RouteData.
		/// </summary>
		/// <typeparam name="T">Expected controller type.</typeparam>
		/// <param name="route">Route information to be verified.</param>
		/// <param name="action">Expected action name.</param>
		/// <returns>Parameters to be verified.</returns>
		public static dynamic MapTo<T>(this RouteData route, string action)
			where T : Controller
		{
			var typeName	= typeof(T).Name;
			var index		= typeName.LastIndexOf("Controller");
			var controller	= typeName.Substring(0, index);
			return route.MapTo(controller, action);
		}


		/// <summary>
		/// Tests the specified URL.
		/// </summary>
		/// <param name="url">URL to be verified.</param>
		/// <param name="controller">Expected controller name.</param>
		/// <param name="action">Expected action name.</param>
		/// <returns>Parameters to be verified.</returns>
		public static dynamic MapTo(this string url, string controller, string action)
		{
			return url.Route().MapTo(controller, action);
		}


		/// <summary>
		/// Tests the specified RouteData.
		/// </summary>
		/// <param name="route">Route information to be verified.</param>
		/// <param name="controller">Expected controller name.</param>
		/// <param name="action">Expected action name.</param>
		/// <returns>Parameters to be verified.</returns>
		public static dynamic MapTo(this RouteData route, string controller, string action)
		{
			//--- Test route
			Assert.IsNotNull(route, "RouteData should be found.");

			//--- Test controller and action
			var parameters = new RouteDataParameters(route);
			parameters.Test("controller", controller);
			parameters.Test("action", action);

			//--- Test arguments
			return new RouteDataParameters(route);
		}
		#endregion


		#region Expression Tree
		/// <summary>
		/// Tests the specified URL.
		/// </summary>
		/// <typeparam name="T">Expected controller type.</typeparam>
		/// <param name="url">URL to be verified.</param>
		/// <param name="action">Expected action.</param>
		public static void MapTo<T>(this string url, Expression<Func<T, ActionResult>> action)
			where T : Controller
		{
			url.Route().MapTo<T>(action);
		}


		/// <summary>
		/// Tests the specified RouteData.
		/// </summary>
		/// <typeparam name="T">Expected controller type.</typeparam>
		/// <param name="route">Route information to be verified.</param>
		/// <param name="action">Expected action.</param>
		public static void MapTo<T>(this RouteData route, Expression<Func<T, ActionResult>> action)
			where T : Controller
		{
			//--- Test controller and action
			var body		= (MethodCallExpression)action.Body;
			var method		= body.Method;
			var parameters	= route.MapTo<T>(method.Name) as RouteDataParameters;

			//--- Test parameters
			Func<Expression, object> GetValue = exp =>
			{
				switch (exp.NodeType)
				{
					case ExpressionType.Call:
					case ExpressionType.Invoke:
					case ExpressionType.MemberAccess:
					case ExpressionType.New:		return Expression.Lambda(exp).Compile().DynamicInvoke();
					case ExpressionType.Constant:	return ((ConstantExpression)exp).Value;
					default:						return null;
				}
			};
			method.GetParameters().Select((x, i) => new
			{
				Name	= x.Name,
				Expect	= GetValue(body.Arguments[i]),
			})
			.ForEach(x => parameters.Test(x.Name, x.Expect));
		}
		#endregion


		#region Helper Methods
		/// <summary>
		/// Gets RouteData from the specified URL.
		/// </summary>
		/// <param name="url">Relative URL like "~/Home/Index"</param>
		/// <returns>Route information.</returns>
		private static RouteData Route(this string url)
		{
			//--- Test url
			Assert.IsNotNull(url, "URL should not be null.");

			//--- Remove query string
			url = url.Split('?')[0];

			//--- Get route information using stub HTTP context
			var context	= new Mock<HttpContextBase>();
			context.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
			return RouteTable.Routes.GetRouteData(context.Object);
		}
		#endregion
	}
}
