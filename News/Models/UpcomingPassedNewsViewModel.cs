using System.Collections.Generic;

namespace News.Models
{
    public class UpcomingPassedNewsViewModel
    {
        public IEnumerable<NewsViewModel> LatestNews { get; set; }

        public IEnumerable<NewsViewModel> OldNews { get; set; }
    }
}