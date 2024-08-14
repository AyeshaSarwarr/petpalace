using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using petpalace.Models;


namespace petpalace.Controllers
{
    public class BuyerController : Controller
    {
        private BuyerRepository _buyerRepository = new BuyerRepository();

        // Action for login view
        public IActionResult Login()
        {
            return View();
        }

        // Action to handle login form submission
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var buyer = _buyerRepository.Login(username, password);

            if (buyer != null)
            {
                string data = string.Empty;

                if (HttpContext.Request.Cookies.ContainsKey("First_Request"))
                {
                    string firstVisitedDateTime = HttpContext.Request.Cookies["First_Request"];
                    data = "Welcome back! You visited on: " + firstVisitedDateTime;
                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true
                    };
                    HttpContext.Response.Cookies.Append("First_Request", DateTime.Now.ToString(), cookieOptions);
                    data = "You are visiting for the first time!";
                }

                ViewData["Message"] = data;
                return RedirectToAction("Index");
            }

            ViewData["Error"] = "Invalid username or password.";
            return View();
        }

        // Action for signup view
        public IActionResult Signup()
        {
            return View();
        }

        // Action to handle signup form submission
        [HttpPost]
        public IActionResult Signup(string username, string password, string fullName, string email, string phoneNumber)
        {
            if (_buyerRepository.UserExists(username))
            {
                ViewData["Error"] = "Username already exists. Please choose a different username.";
                return View();
            }

            BuyerEntity newBuyer = new BuyerEntity
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            _buyerRepository.Signup(newBuyer);

            return RedirectToAction("Index");
        }

        // Action for the index view
        public IActionResult Index()
        {
            string data = string.Empty;

            if (HttpContext.Request.Cookies.ContainsKey("First_Request"))
            {
                string firstVisitedDateTime = HttpContext.Request.Cookies["First_Request"];
                data = "Welcome back! You visited on: " + firstVisitedDateTime;
            }
            else
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true
                };
                HttpContext.Response.Cookies.Append("First_Request", DateTime.Now.ToString(), cookieOptions);
                data = "You are visiting for the first time!";
            }

            ViewData["Message"] = data;
            return View();
        }
    }
}
