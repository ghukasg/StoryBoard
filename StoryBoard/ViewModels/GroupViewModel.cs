using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoryBoard.ViewModels
{
    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsMember { get; set; }

        public int NumberOfStories { get; set; }
        public int NumberOfMembers { get; set; }

        public string OwnerId { get; set; }
    }
}