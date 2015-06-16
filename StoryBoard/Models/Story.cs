using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryBoard.Models
{
    public class Story
    {
        public int StoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PostedOn { get; set; }
        
        public ApplicationUser User { get; set; }
       
        public virtual ICollection<Group> Groups { get; set; }
    }
}