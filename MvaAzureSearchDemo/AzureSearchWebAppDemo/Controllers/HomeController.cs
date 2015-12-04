using System.Configuration;
using System.Web.Mvc;

namespace AzureSearchWebAppDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];

            return View();
        }
    }
}