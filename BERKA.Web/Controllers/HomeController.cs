using System.Diagnostics;
using BERKA.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BERKA.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
