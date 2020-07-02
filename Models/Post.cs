using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForTheFamApp.Models
{
    public class Post
    {
        [Key] // Primary Key
        public int PostId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        [MaxLength(45, ErrorMessage = "Must be less than 45 characters.")]
        public string Topic { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        public string AmazonProductUrl { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "A product name is required.")]
        public string ProductName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Must be more than 2 characters.")]
        public string Body { get; set; }

        public string ImgUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Foreign Keys:
        // FK, must be named same as the primary key it relates to
        public int UserId { get; set; } // 1 User : many Post & 1 User : Many ForumPost

        public int ProductPostId { get; set; } // 1 ProductPostId : 1 PostId

        // Navigation properties:
        public User Author { get; set; }
    }
}