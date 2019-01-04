using System;
using System.Collections.Generic;

namespace blog.Core.Entities
{
    public class Tag :Entity
    {
        //public int Id { get; set; }
        public string TagName { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
    }
}
