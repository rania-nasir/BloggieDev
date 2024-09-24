using ASP.NET_MVC_Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_MVC_Application.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string currentUser = User.Identity.Name;

            // Fetch posts that are either visible to all or belong to the current user
            var posts = db.Posts
                          .Where(p => p.Visible || p.Author == currentUser)
                          .Include(p => p.PostTechStacks.Select(pt => pt.TechStack)) // Include related TechStacks
                          .ToList();

            // Create HomeIndexViewModel and assign the posts to it
            var viewModel = new HomeIndexViewModel
            {
                Posts = posts
            };

            return View(viewModel);
        }

        // Action to provide suggestions for the autocomplete
        [HttpGet]
        public JsonResult SearchSuggestions(string term)
        {
            var postTitles = db.Posts
                .Where(p => p.Heading.Contains(term))
                .Select(p => p.Heading)
                .ToList();

            return Json(postTitles, JsonRequestBehavior.AllowGet);
        }

        // Action to search posts by title and return the results
        [HttpGet]
        public ActionResult SearchPosts(string searchTerm)
        {
            var posts = db.Posts
                          .Where(p => p.Heading.Contains(searchTerm))
                          .Include(p => p.PostTechStacks.Select(pt => pt.TechStack)) // Include related TechStacks
                          .ToList();

            return PartialView("_IndexPartial", posts);  // Return the partial view to render the search results
        }

[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}