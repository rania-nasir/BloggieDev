using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_Application.Models
{
    public class EditRoleViewModel
    {
        [Required]
        [Display(Name = "Select Role to Update")]
        public string SelectedRoleName { get; set; }

        [Required]
        [Display(Name = "New Role Name")]
        public string NewRoleName { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}