using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using News.Entities;
using News.Entities.Data;

namespace News.Entities
{    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Data.News> Newses { get; set; }
        public IDbSet<Comment> Comments { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}