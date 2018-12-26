using System;
using System.Collections.Generic;

namespace blog.Infrastructure.ViewModels
{
    public class PostTagViewModel
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public IEnumerable<string> TagName { get; set; }
    }
}
