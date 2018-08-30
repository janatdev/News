using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News.Models
{
    public class UpcomingPassedNewsViewModel
    {
        public IEnumerable<NewsViewModel> LatestNews { get; set; }

        public IEnumerable<NewsViewModel> OldNews { get; set; }
    }
}