using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using News.Models;

namespace News.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var newses = this.context.Newses
                .OrderBy(e => e.StartDateTime)
                .Where(e => e.IsPublic)
                .Select(NewsViewModel.ViewModel);

            var upcomingNews = newses.Where(e => e.StartDateTime > DateTime.Now);
            var pastNews = newses.Where(e => e.StartDateTime <= DateTime.Now);

            return View(new UpcomingPassedNewsViewModel()
            {
                LatestNews = upcomingNews,
                OldNews = pastNews
            });
        }

        public ActionResult NewsDetailsById(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var newsDetails = this.context.Newses
                .Where(e => e.Id == id)
                .Where(e => e.IsPublic || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                .Select(NewsDetailsViewModel.ViewModel)
                .FirstOrDefault();

            var isOwner = (newsDetails != null && newsDetails.AuthorId != null &&
                           newsDetails.AuthorId == currentUserId);
            return this.PartialView("_NewsDetails", newsDetails);

        }
    }
}