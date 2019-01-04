using System;
using System.Collections.Generic;
using blog.Core.Entities;

namespace blog.Infrastructure.ViewModels
{
    public class PostUpdateViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
