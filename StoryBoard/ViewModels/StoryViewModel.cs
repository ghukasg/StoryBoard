using System;
using System.Collections.Generic;
using StoryBoard.Models;

namespace StoryBoard.ViewModels
{
    public class StoryViewModel
    {
        public int StoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime PostedOn { get; set; }
        public string UserId { get; set; }

        public int[] Groups { get; set; }
    }
}