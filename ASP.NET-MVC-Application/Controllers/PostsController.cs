using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC_Application.Models;

namespace ASP.NET_MVC_Application.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        public async Task<ActionResult> Details(Guid id)
        {
            // Fetch post with associated PostTechStacks and TechStacks
            var post = await db.Posts
                .Include(p => p.PostTechStacks.Select(pt => pt.TechStack))  // Eager loading PostTechStacks and TechStack
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.TechStackList = new MultiSelectList(db.TechStacks, "Id", "Name");
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post model, Guid[] selectedTechStacks)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;

                // Handle image upload
                if (model.FeaturedImage != null && model.FeaturedImage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(model.FeaturedImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    model.FeaturedImage.SaveAs(path);
                }

                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Heading = model.Heading,
                    PageTitle = model.PageTitle,
                    Content = model.Content,
                    ShortDescription = model.ShortDescription,
                    FeaturedImageUrl = fileName,
                    PublishedDate = DateTime.Now,
                    Author = User.Identity.Name,
                    Visible = model.Visible
                };

                db.Posts.Add(post);

                if (selectedTechStacks != null)
                {
                    foreach (var techStackId in selectedTechStacks)
                    {
                        db.PostTechStacks.Add(new PostTechStack
                        {
                            PostId = post.Id,
                            TechStackId = techStackId
                        });
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TechStackList = new MultiSelectList(db.TechStacks, "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = await db.Posts
                .Include(p => p.PostTechStacks.Select(pt => pt.TechStack))
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            if (post.Author != User.Identity.Name)
            {
                return new HttpUnauthorizedResult(); // Return unauthorized if the user is not the author
            }

            // Load selected tech stacks for the post
            var selectedTechStacks = post.PostTechStacks.Select(pt => pt.TechStackId).ToArray();
            ViewBag.TechStackList = new MultiSelectList(db.TechStacks, "Id", "Name", selectedTechStacks);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Post post, Guid[] selectedTechStacks)
        {
            if (ModelState.IsValid)
            {
                // Find the existing post in the database
                var existingPost = await db.Posts
                    .Include(p => p.PostTechStacks)
                    .FirstOrDefaultAsync(p => p.Id == post.Id);

                // Ensure the post exists and that the current user is the author
                if (existingPost == null || existingPost.Author != User.Identity.Name)
                {
                    return new HttpUnauthorizedResult(); // Return unauthorized if the user is not the author
                }

                string fileName = existingPost.FeaturedImageUrl; // Keep the existing image if no new image is uploaded

                // Handle image upload
                if (post.FeaturedImage != null && post.FeaturedImage.ContentLength > 0)
                {
                    fileName = Path.GetFileName(post.FeaturedImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    post.FeaturedImage.SaveAs(path); // Save the new image
                }

                // Update post details
                existingPost.Heading = post.Heading;
                existingPost.PageTitle = post.PageTitle;
                existingPost.Content = post.Content;
                existingPost.ShortDescription = post.ShortDescription;
                existingPost.FeaturedImageUrl = fileName; // Update image URL
                existingPost.Visible = post.Visible;

                // Update the associated TechStacks
                if (selectedTechStacks != null)
                {
                    // Remove existing PostTechStacks
                    db.PostTechStacks.RemoveRange(existingPost.PostTechStacks);

                    // Add new PostTechStacks
                    foreach (var techStackId in selectedTechStacks)
                    {
                        db.PostTechStacks.Add(new PostTechStack
                        {
                            PostId = existingPost.Id,
                            TechStackId = techStackId
                        });
                    }
                }

                // Mark the entity as modified
                db.Entry(existingPost).State = EntityState.Modified;
                await db.SaveChangesAsync(); // Save changes asynchronously

                return RedirectToAction("Index", "Home");
            }

            // If the ModelState is invalid, reload the available tech stacks and return the view
            ViewBag.TechStackList = new MultiSelectList(db.TechStacks, "Id", "Name", selectedTechStacks);
            return View(post);
        }




        [HttpGet]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            if (post.Author != User.Identity.Name)
            {
                return new HttpUnauthorizedResult(); // Return unauthorized if the user is not the author
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Post post = db.Posts.Find(id);

            if (post == null || post.Author != User.Identity.Name)
            {
                return new HttpUnauthorizedResult(); // Return unauthorized if the user is not the author
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
