using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace News.Models
{
    public class NewsDetailsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Entities.Data.News, NewsDetailsViewModel>> ViewModel
        {
            get
            {
                return e=>new NewsDetailsViewModel()
                {
                    Id = e.Id,
                    Description = e.Description,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.Author.Id
                };
            }
        }
    }
}