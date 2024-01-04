﻿namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        // one to many
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
