using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
    }
}