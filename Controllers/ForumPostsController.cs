using System;
using System.Collections.Generic;
using System.Linq;
using ForTheFamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForTheFamApp.Controllers
{
    public class ForumPostsController : Controller
    {
        private ForTheFamAppContext db;
        public ForumPostsController(ForTheFamAppContext context)
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

        [HttpGet("famforum")]
        public IActionResult FamForum()
        {
            List<ForumPost> allForumPosts = db.ForumPosts
                .Include(post => post.Author)
                .ToList();

            return View("Forum", allForumPosts);
        }

        // handles POST request form submission to CREATE a new Post model instance
        [HttpPost("/forumposts/create")]
        public IActionResult Create(ForumPost newForumPost)
        {
            if (ModelState.IsValid == false)
            {
                // send back to the page with the form so error messages are displayed
                return View("FamForum");
            }

            // ModelState IS valid
            newForumPost.UserId = (int)uid;
            db.ForumPosts.Add(newForumPost);

            db.SaveChanges();


            return RedirectToAction("FamForum");
        }



        [HttpGet("/forumposts/{id}/edit")]
        public IActionResult Edit(int id)
        {

            if (!isLoggedIn)
            {
                RedirectToAction("Index", "Home");
            }

            ForumPost selectedPost = db.ForumPosts.FirstOrDefault(post => post.ForumPostId == id);

            // in case manually typing url into address bar, bypassing hidden edit button
            if (selectedPost == null || selectedPost.UserId != uid)
            {
                return RedirectToAction("FamForum");
            }

            return View("EditForumPost", selectedPost);
        }



        [HttpPost("/forumposts/{id}/update")]
        public IActionResult UpdateForumPost(ForumPost editedPost, int id)
        {

            if (ModelState.IsValid == false)
            {
                editedPost.ForumPostId = id;
                return View("EditForumPost", editedPost);
            }

            ForumPost selectedPost = db.ForumPosts.FirstOrDefault(post => post.ForumPostId == id);

            if (selectedPost == null)
            {
                return RedirectToAction("FamForum");
            }

            selectedPost.ForumMsg = editedPost.ForumMsg;
            selectedPost.ImgUrl = editedPost.ImgUrl;
            selectedPost.UpdatedAt = DateTime.Now;

            db.ForumPosts.Update(selectedPost);
            db.SaveChanges();

            return RedirectToAction("FamForum");
        }



        [HttpGet("/forumposts/{id}/delete")]
        public IActionResult Delete(int id)
        {
            ForumPost selectedPost = db.ForumPosts.FirstOrDefault(post => post.ForumPostId == id);

            if (selectedPost != null)
            {
                db.ForumPosts.Remove(selectedPost);
                db.SaveChanges();
            }

            return RedirectToAction("FamForum");
        }


    }
}