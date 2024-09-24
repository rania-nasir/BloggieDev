using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_Application.Models
{
    public class DeleteRoleViewModel
    {
        //[Required]
        //public string RoleId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string SelectedRoleName { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}