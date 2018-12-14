using System;
using System.Collections.Generic;

namespace blog.Core.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Gender { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
