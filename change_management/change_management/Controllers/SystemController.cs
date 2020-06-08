using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace change_management.Controllers
{
    public class SystemController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public SystemController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Systems()
        {
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            List<SystemEntity> systems = DatabaseService.databaseSelectSystems("systems");
            ViewData["Message"] = "System management page.";

            return View(systems);
        }

        public IActionResult AddSystem()
        {
            return View();
        }

        public IActionResult SubmitNewSystem(SystemEntity system)
        {
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            DatabaseService.databaseSystemInsert(system);
            return RedirectToAction("Systems");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}