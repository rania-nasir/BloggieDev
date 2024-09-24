using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_Application.Models
{
    public class PostTechStack
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid TechStackId { get; set; }
        public TechStack TechStack { get; set; }
    }
}