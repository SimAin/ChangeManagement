using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using change_management.Models;
using change_management.Models.ViewModels;
using change_management.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;

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
            UserDatabaseService dbService = new UserDatabaseService(_configuration);
            List<User> users = dbService.SelectAll().ToList();

            return View(users);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult userLogin(LoginViewModel u)
        {
            UserDatabaseService dbService = new UserDatabaseService(_configuration);
            var activeUser = dbService.Select(Convert.ToInt32(u.userId));
            SessionService.loggedInUser = activeUser;
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddUser()
        {
            return View();
        }

        public IActionResult SubmitNewUser(User user)
        {
            UserDatabaseService dbService = new UserDatabaseService(_configuration);
            dbService.Insert(new User(user.forename, user.surname, user.role));
            return RedirectToAction("Users");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
