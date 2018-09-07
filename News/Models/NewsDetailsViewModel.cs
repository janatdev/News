using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.Ajax.Utilities;
using News.Entities.Data;

namespace News.Models
{
    public class NewsDetailsViewModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public int Likes { get; set; }

        public static Expression<Func<Entities.Data.News, NewsDetailsViewModel>> ViewModel
        {
            get
            {
                return e=>new NewsDetailsViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    AuthorName = e.Author.FullName,
                    Description = e.Description,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.Author.Id,      
                    Likes = e.Likes
                };
            }
        }       
    }
}