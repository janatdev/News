using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.Ajax.Utilities;
using News.Entities.Data;

namespace News.Models
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public string Author { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return e=> new CommentViewModel()
                {
                    Text = e.Text,
                    Author = e.Author.FullName
                };
            }
        }

    }
}