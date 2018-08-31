using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using News.Entities;

namespace News.Controllers
{
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        protected ApplicationDbContext context = new ApplicationDbContext();

        public bool IsAdmin()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = (currentUserId != null && this.User.IsInRole("Administrator"));
            return isAdmin;
        }
    }
}