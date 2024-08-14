using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using petpalace.Models;

namespace petpalace.Controllers
{
    public class SellerController : Controller
    {
        private static List<SellerEntity> sellers = new List<SellerEntity>();

        // Action for login view
        public IActionResult Login()
        {
            return View();
        }

        // Action to handle login form submission
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var seller = CheckSellerCredentials(username, password);

            if (seller != null)
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
        public IActionResult Signup(string username, string password, string businessName, string businessAddress, string contactNumber, string website)
        {
            var result = RegisterSeller(username, password, businessName, businessAddress, contactNumber, website);

            if (result)
            {
                return RedirectToAction("Index");
            }

            ViewData["Error"] = "Signup failed. Please try again.";
            return View();
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

        // Method to check seller credentials
        private SellerEntity CheckSellerCredentials(string username, string password)
        {
            foreach (var seller in sellers)
            {
                if (seller.Username == username && seller.Password == password)
                {
                    return seller;
                }
            }
            return null; // Return null if credentials are invalid
        }

        // Method to register a new seller
        private bool RegisterSeller(string username, string password, string businessName, string businessAddress, string contactNumber, string website)
        {
            // Check if the username already exists
            foreach (var seller in sellers)
            {
                if (seller.Username == username)
                {
                    return false; // Username already exists
                }
            }

            // Add new seller to the list
            sellers.Add(new SellerEntity
            {
                Username = username,
                Password = password,
                BusinessName = businessName,
                BusinessAddress = businessAddress,
                ContactNumber = contactNumber,
                Website = website
            });

            return true; // Registration successful
        }
    }
}
