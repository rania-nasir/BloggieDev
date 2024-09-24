using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_Application.Models
{
    public class AssignRoleViewModel
    {
        [Required]
        [Display(Name = "Select User")]
        public string SelectedUserId { get; set; }

        [Required]
        [Display(Name = "Select Role")]
        public string SelectedRoleName { get; set; }

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}