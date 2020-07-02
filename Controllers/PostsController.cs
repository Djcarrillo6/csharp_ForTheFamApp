using System;
using System.Collections.Generic;
using System.Linq;
using ForTheFamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace ForTheFamApp.Controllers
{
    public class PostsController : Controller
    {
        // private field of controller class
        private ForTheFamAppContext db;
        public PostsController(ForTheFamAppContext context)
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

        [HttpGet("/posts")]
        public IActionResult Dashboard(int id)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Post> allPosts = db.Posts
                .Include(post => post.Author)
                .ToList();


            return View("Dashboard", allPosts);
        }


        [HttpGet("/posts/new")]
        public IActionResult New()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("New");
        }

        // handles POST request form submission to CREATE a new Post model instance
        [HttpPost("/posts/create")]
        public IActionResult Create(Post newPost)
        {
            if (ModelState.IsValid == false)
            {

                return View("New");
            }

            // ModelState IS valid
            newPost.UserId = (int)uid;
            db.Posts.Add(newPost);

            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("/posts/{id}")]
        public IActionResult Details(int id)
        {
            if (!isLoggedIn)
            {
                RedirectToAction("Index");
            }

            // .include is dealing with a post, .thenInclude is dealing with whatever was just included above it
            Post selectedPost = db.Posts
                .Include(post => post.Author)
                .FirstOrDefault(post => post.PostId == id);

            List<ProductPost> allProductPostsForThisPostId = db.ProductPosts
            .Include(prodpost => prodpost.Author)
            .ToList();

            ViewBag.AllProductPostsForThisPostId = allProductPostsForThisPostId;

            if (selectedPost == null)
            {
                return RedirectToAction("All");
            }

            return View("Details", selectedPost);
        }



        [HttpGet("/posts/{id}/edit")]
        public IActionResult Edit(int id)
        {

            if (!isLoggedIn)
            {
                RedirectToAction("Index", "Home");
            }

            Post selectedPost = db.Posts.FirstOrDefault(post => post.PostId == id);

            // in case manually typing url into address bar, bypassing hidden edit button
            if (selectedPost == null || selectedPost.UserId != uid)
            {
                return RedirectToAction("Dashboard");
            }

            return View("Edit", selectedPost);
        }

        [HttpPost("/posts/{id}/update")]
        public IActionResult Update(Post editedPost, int id)
        {

            if (ModelState.IsValid == false)
            {
                editedPost.PostId = id;
                return View("Edit", editedPost);
            }

            Post selectedPost = db.Posts.FirstOrDefault(post => post.PostId == id);

            if (selectedPost == null)
            {
                return RedirectToAction("Dashboard");
            }

            selectedPost.Topic = editedPost.Topic;
            selectedPost.Body = editedPost.Body;
            selectedPost.AmazonProductUrl = editedPost.AmazonProductUrl;
            selectedPost.ImgUrl = editedPost.ImgUrl;
            selectedPost.UpdatedAt = DateTime.Now;

            db.Posts.Update(selectedPost);
            db.SaveChanges();

            return RedirectToAction("Dashboard", new { id = selectedPost.PostId });
        }

        [HttpGet("/posts/{id}/delete")]
        public IActionResult Delete(int id)
        {
            Post selectedPost = db.Posts.FirstOrDefault(post => post.PostId == id);

            if (selectedPost != null)
            {
                db.Posts.Remove(selectedPost);
                db.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }



    }
}