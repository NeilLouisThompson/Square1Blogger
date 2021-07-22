using System;
using System.Collections.Generic;

namespace Square1Blogger.Models.Blogs
{
    public class StreamBlogItems
    {
        public string title { get; set; }
        public string description { get; set; }
        public string publication_date { get; set; }
    }

    public class StreamBlog
    {
        public List<StreamBlogItems> data { get; set; }
    }
}
