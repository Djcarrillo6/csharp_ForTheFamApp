using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ForTheFamApp.Models
{
    public class ForumComment
    {
        [Key] // Pk
        public int ForumCommentId { get; set; }

        [Required]
        [Display(Name = "Forum Post Msg")]
        [MinLength(1, ErrorMessage = "Must be more than 1 characters.")]
        public string Comment { get; set; }

        public string ImgUrl { get; set; }

        // Foreign Keys:
        // FK, must be named same as the primary key it relates to
        public int UserId { get; set; } // 1 User : many ForumPost

        // Navigation properties:
        public User Author { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}