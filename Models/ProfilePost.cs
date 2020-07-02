using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForTheFamApp.Models
{
    public class ProfilePost
    {
        [Key] // Pk
        public int ProfilePostId { get; set; }

        [Required]
        [Display(Name = "Forum Post Msg")]
        [MinLength(1, ErrorMessage = "Must be more than 1 characters.")]
        public string ForumMsg { get; set; }

        public int UserProfileId { get; set; }

        public int ProfilePostAuthor { get; set; }


        // Navigation properties:
        public User Author { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}