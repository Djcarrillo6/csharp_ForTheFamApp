using System;
using System.Collections.Generic;
using System.Linq;
using ForTheFamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ForTheFamApp.Controllers
{
    public class ProductPostsController : Controller
    {
        private ForTheFamAppContext db;
        public ProductPostsController(ForTheFamAppContext context)
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



        // // handles POST request form submission to CREATE a new Post model instance
        // [HttpPost("/productposts/create")]
        // public IActionResult Create(ProductPost newProductPost)
        // {
        //     if (ModelState.IsValid == false)
        //     {
        //         // send back to the page with the form so error messages are displayed
        //         return View("Details");
        //     }

        //     // ModelState IS valid
        //     newProductPost.UserId = (int)uid;
        //      newProductPost.PostId = 
        //     db.ProductPosts.Add(newProductPost);

        //     db.SaveChanges();


        //     return RedirectToAction("Product", new { id = newProductPost.ProductPostId });
        // }

    }
}