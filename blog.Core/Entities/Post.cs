using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace blog.Core.Entities
{
    public class Post : Entity
    {
        //private static readonly char delimiter = ';';

        //public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModify { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        //private string _tags;
        //[NotMapped]
        //public string[] Tags 
        //{
        //    get { return _tags.Split(delimiter); }
        //    set
        //    {
        //        _tags = string.Join($"{delimiter}", value);
        //    }
        //}

    }
}
