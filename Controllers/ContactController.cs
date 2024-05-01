using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{    //GET:/<controller>/
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
