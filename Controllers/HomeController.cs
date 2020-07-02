using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForTheFamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ForTheFamApp.Controllers
{
    public class HomeController : Controller
    {
        private ForTheFamAppContext db;
        public HomeController(ForTheFamAppContext context)
        {
            db = context;
        }
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            if (isLoggedIn)
            {
                RedirectToAction("Dashboard", "Posts");
            }

            return View();
        }


        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost("/register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "is taken");
                }
            }

            // in case any above custom errors were added
            if (ModelState.IsValid == false)
            {
                // so error messages will be displayed
                return View("Index");
            }

            // hash pw
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("FullName", newUser.FullName());
            return RedirectToAction("Dashboard", "Posts");
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {
            // don't reveal what they got wrong in case they are a hacker
            // but you can temporarily use more specific error messages for testing so you know what went wrong
            string genericErrMsg = "Invalid Email or Password";

            if (ModelState.IsValid == false)
            {
                // display validation errors
                return View("Index");
            }

            User dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

            if (dbUser == null)
            {
                ModelState.AddModelError("LoginEmail", genericErrMsg);
                return View("Index");
            }

            // user found b/c the above return didn't run
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            // go to definition of PasswordVerificationResult to see what result ints mean
            PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if (pwCompareResult == 0)
            {
                ModelState.AddModelError("LoginEmail", genericErrMsg);
                return View("Index");
            }

            // no returns happened, no errors
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("FullName", dbUser.FullName());
            return RedirectToAction("Dashboard", "Posts");
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
