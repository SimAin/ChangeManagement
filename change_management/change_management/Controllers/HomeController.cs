using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using Microsoft.Extensions.Configuration;

namespace change_management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public HomeController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
