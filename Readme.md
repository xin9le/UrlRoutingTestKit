## Summary

Get the shortest and declarative test code you use it!!

You can start writing URL routing test code from "URL.MapTo" method. Subsequently, you can verify the parameters declaratively using the dynamic chain. If you use the overload using expression tree, you can also write as type-safe.



## Samples
#### This library makes you happy!!

This is a certain route definition.

    routes.MapRoute
    (
        name:     "Diary",
        url:      "{controller}/{action}/{year}/{month}/{day}",
        defaults: new { controller = "Person", action = "Diary" }
    );


You can write like a following simple url routing test code.

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
        url.MapTo("Person", "Diary").year(2013).month(1).day(26);  //--- dynamic chain!!
    }

    [TestMethod]
    public void ExpressionTree()
    {
        var url = "~/Person/Diary/2013/1/26";
        url.MapTo<PersonController>(c => c.Diary(2013, 1, 26));  //--- type-safe!!
    }

## Typical & legacy code

    [TestMethod]
    public void Typical()
    {
        //--- test target URL
        var url = "~/Person/Diary/2013/1/26";

        //--- get the route information for the URL
        var context = new Mock<HttpContextBase>();
        context.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
        var routeData = RouteTable.Routes.GetRouteData(context.Object);

        //--- assert
        Assert.AreEqual("Person", routeData.Values["controller"]);
        Assert.AreEqual("Diary",  routeData.Values["action"]);
        Assert.AreEqual("2013",   routeData.Values["year"]);
        Assert.AreEqual("1",      routeData.Values["month"]);
        Assert.AreEqual("26",     routeData.Values["day"]);
    }
