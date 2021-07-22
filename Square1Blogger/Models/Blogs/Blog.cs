using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Square1Blogger.Models.Blogs
{
    [Index(nameof(UserId))]
    public class Blog
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
