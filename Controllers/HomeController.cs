using Microsoft.AspNetCore.Mvc;
using Sign_inWithGGAcc.Models;
using System.Diagnostics;

namespace Sign_inWithGGAcc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login(InternalUserModel.InternalUser user)
        {
            string t = "Login không thành công";
            var context = new DBContext();
            var internalUsers = context.InternalUser.ToList();
            int id=0;
            for (int i = 0; i < internalUsers.Count; i++)
            {
                if (user.Email == internalUsers[i].Email && InternalUserModel.HashCode(user.Password) == internalUsers[i].HashCode)
                {
                    t = "Longin Thành Công";
                    id =i; 
                    break;
                }
            }

            ViewData["FamilyName"] = internalUsers[id].FamilyName;
            ViewData["GivenName"] = internalUsers[id].GivenName;
            ViewData["Result"] = t;
            return View();
        }
    }
}