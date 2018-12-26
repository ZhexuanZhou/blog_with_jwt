using System;
using System.Collections.Generic;

namespace blog.Infrastructure.ViewModels
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModify { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}
