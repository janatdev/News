using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace News.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }

        public static Expression<Func<Entities.Data.News, NewsViewModel>> ViewModel
        {
            get
            {
                return e => new NewsViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Author = e.Author.FullName,
                    Location = e.Location
                };
            }
        }
    }    
}