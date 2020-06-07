using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace change_management.Controllers
{
 public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public UserController(IConfiguration configuration){
            _configuration = configuration;
        }

        public IActionResult Users()
        {
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            List<User> users = DatabaseService.databaseSelect("users");
            ViewData["Message"] = "User management page.";

            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult SubmitNewUser(User user)
        {
            DatabaseService DatabaseService = new DatabaseService(_configuration);
            DatabaseService.databaseUserInsert(user.forename, user.surname, user.role);
            return RedirectToAction("Users");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
