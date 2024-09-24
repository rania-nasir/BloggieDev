using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_Application.Models
{
    public class TechStack
    {
        public Guid Id { get; set; }

        [Display(Name = "Technology Stack")]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<PostTechStack> PostTechStacks { get; set; } // Many-to-Many relationship
    }
}