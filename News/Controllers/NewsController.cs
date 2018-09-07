using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using News.Entities;
using News.Entities.Data;
using News.Models;

namespace News.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {      
        public async Task<ActionResult> Index()
        {
            var news = context.Newses.Include(n => n.Author);
            return View(await news.ToListAsync());
        }

        public ActionResult List()
        {
            using (var context = new ApplicationDbContext())
            {
                var listNewses = context
                    .Newses
                    .Include(p => p.Author)
                    .ToList();

                return View(listNewses);
            }
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await context.Newses.FirstOrDefaultAsync(x=>x.Id== id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: NewsPage/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(context.Users, "Id", "FullName");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,StartDateTime,Duration,AuthorId,Description,Location,IsPublic,Likes")] Entities.Data.News news)
        {
            if (ModelState.IsValid)
            {
                context.Newses.Add(news);
                await context.SaveChangesAsync();
                return RedirectToAction("My");
            }

            ViewBag.AuthorId = new SelectList(context.Users, "Id", "FullName", news.AuthorId);
            return View(news);
        }
              
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await context.Newses.FirstOrDefaultAsync(x=>x.Id==id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(context.Users, "Id", "FullName", news.AuthorId);
            return View(news);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,StartDateTime,Duration,AuthorId,Description,Location,IsPublic,Likes")] Entities.Data.News news)
        {
            if (ModelState.IsValid)
            {
                context.Entry(news).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("My");
            }
            ViewBag.AuthorId = new SelectList(context.Users, "Id", "FullName", news.AuthorId);
            return View(news);
        }


        // GET: NewsPage/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = await context.Newses.FirstOrDefaultAsync(x=>x.Id == id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: NewsPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var news = await context.Newses.FirstOrDefaultAsync(x=>x.Id == id);
            context.Newses.Remove(news);
            await context.SaveChangesAsync();
            return RedirectToAction("My");
        }

        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();

            var newes = this.context.Newses
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(NewsViewModel.ViewModel);

            var upCommingNews = newes.Where(e => e.StartDateTime >= DateTime.Now);
            var pastNews = newes.Where(e => e.StartDateTime <= DateTime.Now);
            var popNews = newes.Where(e => e.Likes > 3);

            return View(new UpcomingPassedNewsViewModel()
            {
                LatestNews = upCommingNews,
                OldNews = pastNews,
                PopularNews = popNews
            });
        }


        public ActionResult Likes(int? id)
        {
            var newses = context.Newses.FirstOrDefault(a => a.Id == id);
            if (newses == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            newses.Likes += 1;
            this.context.SaveChanges();
            return RedirectToAction("My");
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(int newsId, string message, string authorId)
        {            
            var newses = context.Newses.First(x => x.Id == newsId);
            Comment comment = new Comment();
            comment.NewsId = (System.Int32) newsId;
            comment.AuthorId = authorId.ToString();
            comment.Date = DateTime.Now;            
            comment.Text = message;            

            context.Comments.Add(comment);
            context.SaveChanges();
            return RedirectToAction("");
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