using System.Web.Mvc;



namespace SampleApp.Controllers
{
	public class PersonController : Controller
	{
        public ActionResult Diary(int year, int month, int day)
        {
            return View();
        }
	}
}