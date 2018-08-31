using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using News.Entities;
using News.Models;

namespace News.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateNewsModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var e = new Entities.Data.News()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic
                };

                this.context.Newses.Add(e);
                this.context.SaveChanges();                
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var newes = this.context.Newses.Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime).Select(NewsViewModel.ViewModel);

            var upCommingNews = newes.Where(e => e.StartDateTime > DateTime.Now);
                var pastNews = newes.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedNewsViewModel()
            {
                LatestNews = upCommingNews,
                OldNews = pastNews
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var newstoEdit = this.LoadNews(id);
            if (newstoEdit == null)
            {
                return this.RedirectToAction("My");
            }

            //var model = CreateNewsModel.CreateFromNews(newstoEdit);
            return this.View(newstoEdit);
        }

        private Entities.Data.News LoadNews(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var newsToEdit = this.context.Newses
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return newsToEdit;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CreateNewsModel model)
        {
            var newsToEdit = this.LoadNews(id);
            if (newsToEdit == null)
            {
                return this.RedirectToAction("My");
            }

            if (model != null && this.ModelState.IsValid)
            {
                newsToEdit.Title = model.Title;
                newsToEdit.StartDateTime = model.StartDateTime;
                newsToEdit.Duration = model.Duration;
                newsToEdit.Description = model.Description;
                newsToEdit.Location = model.Location;
                newsToEdit.IsPublic = model.IsPublic;

                this.context.SaveChanges();
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (context = new ApplicationDbContext())
            {
                var news = context.Newses.Where(e => e.Id == id);

                if (news != null)
                {
                    return View(news);
                }
                else
                {
                    return HttpNotFound();
                }                
            }            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           /* var newses = context.Newses
                .Where(p => p.Id == id)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .First();
                */
            var newses = context.Newses.First(x => x.Id == id);

            if (newses != null)
            {
                context.Newses.Remove(newses);
                context.SaveChanges();
            }
            
            return RedirectToAction("My");
        }

        protected override void Dispose(bool disposing)
        {
            
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}