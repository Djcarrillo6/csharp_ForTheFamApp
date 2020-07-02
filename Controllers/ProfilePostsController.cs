using System;
using System.Collections.Generic;
using System.Linq;
using ForTheFamApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForTheFamApp.Controllers
{
    public class ProfilePostsController : Controller
    {
        private ForTheFamAppContext db;
        public ProfilePostsController(ForTheFamAppContext context)
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

        [HttpGet("/users/{id}")]
        public IActionResult Profile(int id)
        {

            if (!isLoggedIn)
            {
                RedirectToAction("Index");
            }

            List<ProfilePost> thisUsersProfilePosts = db.ProfilePosts
            .Where(p => p.UserProfileId == id)
            .Include(p => p.Author)
            .ToList();



            ViewBag.ThisUsersProfilePosts = thisUsersProfilePosts;
            ViewBag.UsersProfileId = id;




            User selectedUser = db.Users
                .Include(u => u.Posts)
                .Include(u => u.ForumPosts)
                .Include(u => u.ProfilePosts)
                .FirstOrDefault(user => user.UserId == id);

            // ViewBag.AssociatedUserProfileId = id;

            return View("Profile", selectedUser);
        }

        // handles POST request form submission to CREATE a new Post model instance
        [HttpPost("/profileposts/{userProfileId}/create")]
        public IActionResult Create(ProfilePost newProfilePost, int userProfileId)
        {
            if (ModelState.IsValid == false)
            {
                User selectedProfileId = db.Users
                .Include(user => user.ProfilePosts)
                .ThenInclude(profpost => profpost.Author)
                .FirstOrDefault(user => user.UserId == userProfileId);

                // send back to the page with the form so error messages are displayed
                return View("Profile", selectedProfileId);
            }

            db.ProfilePosts.Add(newProfilePost);

            db.SaveChanges();


            return RedirectToAction("Profile", new { id = newProfilePost.UserProfileId });
        }



        [HttpGet("/profileposts/{id}/edit")]
        public IActionResult Edit(int id)
        {

            if (!isLoggedIn)
            {
                RedirectToAction("Index", "Home");
            }

            ProfilePost selectedPost = db.ProfilePosts.FirstOrDefault(post => post.ProfilePostId == id);

            // in case manually typing url into address bar, bypassing hidden edit button
            if (selectedPost == null || selectedPost.ProfilePostAuthor != uid)
            {
                return RedirectToAction("Profile");
            }

            return View("EditProfilePost", selectedPost);
        }



        [HttpPost("/profileposts/{id}/update")]
        public IActionResult UpdateProfilePost(ProfilePost editedPost, int id)
        {

            if (ModelState.IsValid == false)
            {
                editedPost.ProfilePostId = id;
                return View("EditProfilePost", editedPost);
            }

            ProfilePost selectedPost = db.ProfilePosts.FirstOrDefault(post => post.ProfilePostId == id);

            if (selectedPost == null)
            {
                return RedirectToAction("FamForum");
            }

            selectedPost.ForumMsg = editedPost.ForumMsg;
            selectedPost.UpdatedAt = DateTime.Now;

            db.ProfilePosts.Update(selectedPost);
            db.SaveChanges();

            return RedirectToAction("Profile", new { id = selectedPost.UserProfileId });
        }



        [HttpGet("/profileposts/{id}/delete")]
        public IActionResult Delete(int id, int returnProfileId)
        {
            ProfilePost selectedPost = db.ProfilePosts.FirstOrDefault(post => post.ProfilePostId == id);

            if (selectedPost != null)
            {
                db.ProfilePosts.Remove(selectedPost);
                db.SaveChanges();
            }

            return RedirectToAction("Profile", new { id = returnProfileId });
        }


    }
}