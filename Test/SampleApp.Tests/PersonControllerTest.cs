using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleApp.Controllers;
using System.Web;
using System.Web.Routing;
using UrlRoutingTestKit;



namespace SampleApp.Tests
{
	[TestClass]
	public class PersonControllerTest
	{
		[TestInitialize]
		public void Initialize()
		{
			//--- Setup routes
			RouteTable.Routes.Clear();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}


		[TestMethod]
		public void Dynamic()
		{
			var url = "~/Person/Diary/2013/1/26";
			url.MapTo("Person", "Diary").year(2013).month(1).day(26);
		}


		[TestMethod]
		public void GenericDynamic()
		{
			var url = "~/Person/Diary/2013/1/26";
			url.MapTo<PersonController>("Diary").year(2013).month(1).day(26);
		}


		[TestMethod]
		public void ExpressionTree()
		{
			var url = "~/Person/Diary/2013/1/26";
			url.MapTo<PersonController>(c => c.Diary(2013, 1, 26));
		}


		[TestMethod]
		public void Typical()
		{
			//--- テスト対象のURL
			var url = "~/Person/Diary/2013/1/26";

			//--- URLに対するルート情報の取得
			var context	= new Mock<HttpContextBase>();
			context.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
			var routeData = RouteTable.Routes.GetRouteData(context.Object);

			//--- 結果の診断
			Assert.AreEqual("Person", routeData.Values["controller"]);
			Assert.AreEqual("Diary",  routeData.Values["action"]);
			Assert.AreEqual("2013",	  routeData.Values["year"]);
			Assert.AreEqual("1",	  routeData.Values["month"]);
			Assert.AreEqual("26",	  routeData.Values["day"]);
		}
	}
}