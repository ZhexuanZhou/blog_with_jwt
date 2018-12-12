using System;
namespace blog.Core.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Gender { get; set; }
    }
}
