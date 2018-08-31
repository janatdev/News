using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using News.Entities.Data;

namespace News.Entities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var adminFullName = "Administrator";
            var adminEmail = "admin@admin.com";
            var adminUserName = adminEmail;
            var adminPassword = adminEmail;
            string adminRole = "Administrator";
            
            //CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
            //CreateSeveralNews(context);
        }

        private void CreateSeveralNews(ApplicationDbContext context)
        {
            context.Newses.Add(new Data.News()
            {
                Title = "This Samsung portable NVMe drive is faster than your SSD",
                StartDateTime = DateTime.Now.AddDays(5).AddHours(21).AddMinutes(30),
                Author = context.Users.First(),
            });

            context.Newses.Add(new Data.News()
            {
                Title = "NASA chief says US rockets will launch astronauts from US next year",
                StartDateTime = DateTime.Now.AddDays(4).AddHours(10).AddMinutes(30),
                Author = context.Users.First(),
            });

            context.Newses.Add(new Data.News()
            {
                Title = "Climate change will make hundreds of millions more people nutrient deficient",
                StartDateTime = DateTime.Now.AddDays(7).AddHours(20).AddMinutes(30),
            });

            context.Newses.Add(new Data.News()
            {
                Title = "Eating three chocolate bars a month can reduce risk of heart failure, study claims!",
                StartDateTime = DateTime.Now.AddDays(-2).AddHours(10).AddMinutes(30),
                Duration = TimeSpan.FromHours(1.5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() {Text = "<Unknown> Comment : not registerd user commented on this news post."},
                    new Comment() {Text = "User Comment: Registered user commented on this post.", Author = context.Users.First()}
                }
            });

            context.Newses.Add(new Data.News()
            {
                Title = "Eating Ten chocolate bars a month can reduce risk of heart failure, study claims!",
                StartDateTime = DateTime.Now.AddDays(-4).AddHours(10).AddMinutes(30),
                Duration = TimeSpan.FromHours(3.5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() {Text = "<Unknown> Comment : not registerd user commented on this news post."}                    
                }
            });
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName,
            string adminFullName, string adminPassword, string adminRole)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };

            var userStore = new UserStore<ApplicationUser>(context);

            var userManager = new UserManager<ApplicationUser>(userStore);

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            var userCreate = userManager.Create(adminUser, adminPassword);

            if (!userCreate.Succeeded)
            {
                throw new Exception(string.Join(";", userCreate.Errors));
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreate = roleManager.Create(new IdentityRole(adminRole));

            if (!roleCreate.Succeeded)
            {
                throw new Exception(string.Join(";", roleCreate.Errors));
            }

            var addAdminRole = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRole.Succeeded)
            {
                throw new Exception(string.Join(";", addAdminRole.Errors));
            }
        }
    }
}