using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_Application.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        [Display(Name = "Post Heading")]
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Heading { get; set; }

        [Display(Name = "Page Title")]
        [Required]
        [StringLength(200)]
        public string PageTitle { get; set; }

        [Display(Name = "Content")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(300)]
        public string ShortDescription { get; set; }

        [NotMapped]
        [Display(Name = "Featured Image")]
        public HttpPostedFileBase FeaturedImage { get; set; } // Updated to handle file upload

        public string FeaturedImageUrl { get; set; }

        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Author")]
        [StringLength(50)]
        public string Author { get; set; }

        [Display(Name = "Visible")]
        public bool Visible { get; set; }

        public virtual ICollection<PostTechStack> PostTechStacks { get; set; } // Many-to-Many relationship
    }
}