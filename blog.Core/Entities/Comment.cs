using System;
namespace blog.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }

        public Post Post { get; set; }
        public Author Author { get; set; }
    }
}
